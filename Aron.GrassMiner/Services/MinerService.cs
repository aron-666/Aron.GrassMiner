﻿using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using GrassMiner.Models;
using System.Net;
using System.Drawing;
using Aron.GrassMiner.Models;
using Newtonsoft.Json;
using System.Text;

namespace GrassMiner.Services
{
    public class MinerService : IMinerService
    {
        public ChromeDriver driver { get; set; }
        private readonly AppConfig _appConfig;
        private readonly MinerRecord _minerRecord;
        private bool Enabled { get; set; } = true;

        private Thread? thread;

        private DateTime BeforeRefresh = DateTime.MinValue;
        public MinerService(AppConfig appConfig, MinerRecord minerRecord)
        {
            _appConfig = appConfig;
            this._minerRecord = minerRecord;
            // call https://ifconfig.me to get the public IP address
            try
            {
                _minerRecord.PublicIp = new WebClient().DownloadString("https://ifconfig.me");
            }
            catch (Exception ex)
            {
                _minerRecord.PublicIp = "Error to get your public ip.";
            }

            this.thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        if (Enabled)
                        {
                            Run();
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _minerRecord.Exception = ex.ToString();
                        _minerRecord.ExceptionTime = DateTime.Now;
                        _minerRecord.Status = MinerStatus.Error;
                    }
                    finally
                    {
                        Thread.Sleep(10000);
                    }
                }

            })
            { IsBackground = true };

            this.thread.Start();
        }

        public void Stop()
        {
            Enabled = false;
        }

        public void Start()
        {

            Enabled = true;

        }

        private void Run()
        {
            try
            {
                driver?.Quit();
                driver = null;
                _minerRecord.Status = MinerStatus.AppStart;
                _minerRecord.IsConnected = false;
                _minerRecord.LoginUserName = null;
                _minerRecord.ReconnectSeconds = 0;
                _minerRecord.ReconnectCounts = 0;
                _minerRecord.Exception = null;
                _minerRecord.ExceptionTime = null;
                _minerRecord.Points = "0";
                // get assembly version
                _minerRecord.AppVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

                string userName = _appConfig.UserName;
                string password = _appConfig.Password;

                // 設定 Chrome 擴充功能路徑
                string extensionPath = "./Grass-Extension.crx";
                string chromedriverPath = "./chromedriver";

                // 建立 Chrome 選項
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--chromedriver=" + chromedriverPath);
                if (!_appConfig.ShowChrome)
                    options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--enable-javascript");
                options.AddArgument("--auto-close-quit-quit");
                options.AddArgument("disable-infobars");
                options.AddArgument("--window-size=1024,768");
                if ((_appConfig.ProxyEnable ?? "").ToLower() == "true"
                    && !string.IsNullOrEmpty(_appConfig.ProxyHost))
                {
                    options.AddArgument("--proxy-server=" + _appConfig.ProxyHost);
                    if (!string.IsNullOrEmpty(_appConfig.ProxyUser) && !string.IsNullOrEmpty(_appConfig.ProxyPass))
                    {
                        options.AddArgument($"--proxy-auth={_appConfig.ProxyUser}:{_appConfig.ProxyPass}");
                    }
                }
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_enabled", false);
                options.AddArgument("--disable-gpu"); // 禁用 GPU 加速，减少资源占用
                options.AddArgument("--disable-software-rasterizer"); // 禁用软件光栅化器
                options.AddArgument("--disable-dev-shm-usage"); // 禁用 /dev/shm 临时文件系统
                options.AddArgument("--force-dark-mode");
                options.AddExtension(extensionPath);

                // 建立 Chrome 瀏覽器
                driver = new ChromeDriver(options);
                try
                {
                    driver.Navigate().GoToUrl("https://app.getgrass.io/");
                    Console.WriteLine("Go to app: " + driver.Url);

                    Thread.Sleep(5000);
                    if (!
                    SpinWait.SpinUntil(() =>
                    {
                        try
                        {
                            return driver.PageSource.Contains("Sign In.");

                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }, 30000))
                    {
                        _minerRecord.Status = MinerStatus.LoginError;
                        _minerRecord.Exception = "登入頁面載入錯誤";
                        _minerRecord.ExceptionTime = DateTime.Now;
                        Console.WriteLine("登入頁面載入錯誤，Url: " + driver.Url);
                        return;
                    }

                    // post https://api.getgrass.io/login
                    HttpClient client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://api.getgrass.io/login");
                    request.Headers.Add("Accept", "*/*");
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, br, zstd");
                    request.Headers.Add("Accept-Language", "zh-TW,zh;q=0.9");
                    request.Headers.Add("Origin", "https://app.getgrass.io");
                    request.Headers.Add("Priority", "u=1, i");
                    request.Headers.Add("Referer", "https://app.getgrass.io/");
                    request.Headers.Add("Sec-Ch-Ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
                    request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
                    request.Headers.Add("Sec-Ch-Ua-Platform", "\"Windows\"");
                    request.Headers.Add("Sec-Fetch-Dest", "empty");
                    request.Headers.Add("Sec-Fetch-Mode", "cors");
                    request.Headers.Add("Sec-Fetch-Site", "same-site");
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");

                    var content = JsonConvert.SerializeObject(new
                    {
                        username = userName,
                        password = password
                    });
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");

                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _minerRecord.Status = MinerStatus.LoginError;
                        _minerRecord.Exception = "登入錯誤: " + response.StatusCode;
                        _minerRecord.ExceptionTime = DateTime.Now;
                        Console.WriteLine("登入錯誤，Url: " + driver.Url);
                        return;
                    }
                    GrassLoinResp grassLoinResp = JsonConvert.DeserializeObject<GrassLoinResp>(responseContent);
                    if (grassLoinResp == null || grassLoinResp.result == null || grassLoinResp.result.data == null || string.IsNullOrEmpty(grassLoinResp.result.data.accessToken))
                    {
                        _minerRecord.Status = MinerStatus.LoginError;
                        _minerRecord.Exception = "資料解析錯誤";
                        _minerRecord.ExceptionTime = DateTime.Now;
                        return;
                    }

                    //設定 chrome local storage
                    SetLocalStorageItem(driver, "accessToken", $"\"{grassLoinResp.result.data.accessToken}\"");
                    SetLocalStorageItem(driver, "refreshToken", $"\"{grassLoinResp.result.data.refreshToken}\"");
                    SetLocalStorageItem(driver, "tokenExpiry", DateTimeOffset.UtcNow.AddYears(1).ToUnixTimeSeconds().ToString());
                    SetLocalStorageItem(driver, "userId", $"\"{grassLoinResp.result.data.userId}\"");
                    SetLocalStorageItem(driver, "isAuthenticated", "true");
                    SetLocalStorageItem(driver, "chakra-ui-color-mode", "dark");
                    SetLocalStorageItem(driver, "userColorMode", "\"dark\"");

                    driver.Navigate().GoToUrl("https://api.getgrass.io/");
                    Console.WriteLine("Go to api: " + driver.Url);
                    Thread.Sleep(5000);

                    //等待頁面中包含 "json-formatter-container" class
                    if (!SpinWait.SpinUntil(() =>
                    {
                        try
                        {
                            return driver.PageSource.Contains("json-formatter-container");

                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                    }, 30000))
                    {
                        _minerRecord.Status = MinerStatus.LoginError;
                        _minerRecord.Exception = "api頁面載入錯誤: " + response.StatusCode;
                        _minerRecord.ExceptionTime = DateTime.Now;
                        Console.WriteLine("api頁面載入錯誤，Url: " + driver.Url);

                        return;
                    };

                    //設定 cookie
                    SetCookie(driver, "token", grassLoinResp.result.data.accessToken);



                    driver.Navigate().GoToUrl("https://app.getgrass.io/dashboard");
                    Console.WriteLine("Go to dashboard: " + driver.Url);


                    Thread.Sleep(5000);


                    // 等待包含Refresh
                    DateTime start = DateTime.Now;
                    if (!SpinWait.SpinUntil(() =>
                    {
                        try
                        {
                            DateTime now = DateTime.Now;
                            if ((now - start).TotalSeconds >= 10)
                            {
                                driver.Navigate().GoToUrl("https://app.getgrass.io/dashboard");
                                Console.WriteLine("Go to dashboard: " + driver.Url);
                                start = now;
                            }
                            return driver.PageSource.Contains("Refresh");

                        }
                        catch (Exception ex)
                        {
                            return false;
                        }
                        finally
                        {
                        }
                    }, 60000))
                    {
                        _minerRecord.Status = MinerStatus.LoginError;
                        _minerRecord.Exception = "dashboard頁面載入錯誤: " + response.StatusCode;
                        _minerRecord.ExceptionTime = DateTime.Now;
                        Console.WriteLine("dashboard頁面載入錯誤，Url: " + driver.Url);

                        return;
                    };
                    Thread.Sleep(5000);

                    _minerRecord.LoginUserName = userName;
                }
                catch (Exception ex)
                {
                    _minerRecord.Status = MinerStatus.LoginError;
                    _minerRecord.Exception = ex.ToString();
                    _minerRecord.ExceptionTime = DateTime.Now;
                    Console.WriteLine(ex);
                    return;
                }


                driver.Navigate().GoToUrl("chrome-extension://ilehaonighjijnmpnagapkhpcdbhclfg/index.html");
                Console.WriteLine("Go to extension: " + driver.Url);
                driver.Manage().Window.Size = new Size(1024, 768);

                _minerRecord.Status = MinerStatus.Disconnected;
                while (Enabled)
                {
                    try
                    {
                        if (!driver.PageSource.Contains("Connected"))
                        {
                            driver.FindElement(By.Id("menu-button-:r1:")).Click();
                            driver.FindElement(By.XPath("//*[contains(text(), 'Reconnect')]")).Click();
                            _minerRecord.Status = MinerStatus.Disconnected;
                            _minerRecord.IsConnected = false;
                            _minerRecord.ReconnectCounts++;
                        }
                        else
                        {
                            _minerRecord.Status = MinerStatus.Connected;
                            //$('img[alt="token"]')

                            IWebElement? imageElement = driver.FindElement(By.CssSelector("img[alt='token']"));

                            IWebElement? nextSiblingElement = imageElement?.FindElement(By.XPath("following-sibling::*"));

                            _minerRecord.Points = nextSiblingElement?.Text ?? "";
                            IWebElement element = driver.FindElement(By.XPath("//p[starts-with(., 'Network quality:')]"));
                            _minerRecord.NetworkQuality = element.Text.Replace("Network quality:", "");
                            //IWebElement? userNameElement = driver.FindElement(By.CssSelector("span[title='Username']"));
                            _minerRecord.IsConnected = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        _minerRecord.Status = MinerStatus.Connected;
                    }
                    finally
                    {
                        int countdownSeconds = 30;

                        // 倒數計時
                        while (countdownSeconds > 0)
                        {
                            _minerRecord.ReconnectSeconds = countdownSeconds;

                            SpinWait.SpinUntil(() => false, 1000); // 等待 1 秒
                            if (driver.PageSource.Contains("Connected"))
                                break;
                            countdownSeconds--;
                            if (!Enabled)
                            {
                                break;
                            }
                        }
                        if (Enabled && BeforeRefresh.AddSeconds(60) <= DateTime.Now)
                        {
                            BeforeRefresh = DateTime.Now;
                            //refresh
                            driver.Navigate().GoToUrl("chrome-extension://ilehaonighjijnmpnagapkhpcdbhclfg/index.html");
                            SpinWait.SpinUntil(() => !Enabled, 15000);
                        }
                        Thread.Sleep(1000);
                    }
                }
                _minerRecord.Status = MinerStatus.Stop;
            }
            catch (Exception ex)
            {
                _minerRecord.Exception = ex.ToString();
                _minerRecord.ExceptionTime = DateTime.Now;
                _minerRecord.Status = MinerStatus.Error;
                Console.WriteLine(ex);
            }
            finally
            {
                driver?.Quit();
                driver = null;
            }
        }

        static void SetLocalStorageItem(IWebDriver driver, string key, string value)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript($"window.localStorage.setItem('{key}', '{value}');");
        }

        static void SetCookie(IWebDriver driver, string key, string value)
        {
            driver.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(key, value, "/", DateTime.UtcNow.AddYears(1)));
        }

        static string GetLocalStorageItem(IWebDriver driver, string key)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (string)js.ExecuteScript($"return window.localStorage.getItem('{key}');");
        }

    }
}
