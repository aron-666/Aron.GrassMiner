using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using GrassMiner.Models;
using System.Net;

namespace GrassMiner.Services
{
    public class MinerService
    {
        public ChromeDriver driver;
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
                        Thread.Sleep(30000);
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
                if((_appConfig.ProxyEnable ?? "").ToLower() == "true" 
                    && !string.IsNullOrEmpty(_appConfig.ProxyHost))
                {
                    options.AddArgument("--proxy-server=" + _appConfig.ProxyHost);
                    if(!string.IsNullOrEmpty(_appConfig.ProxyUser) && !string.IsNullOrEmpty(_appConfig.ProxyPass))
                    {
                        options.AddArgument($"--proxy-auth={_appConfig.ProxyUser}:{_appConfig.ProxyPass}");
                    }
                }
                options.AddExcludedArgument("enable-automation");
                options.AddUserProfilePreference("credentials_enable_service", false);
                options.AddUserProfilePreference("profile.password_manager_enabled", false);
                options.AddExtension(extensionPath);

                // 建立 Chrome 瀏覽器
                driver = new ChromeDriver(options);
                try
                {
                    driver.Navigate().GoToUrl("https://app.getgrass.io/");
                    _minerRecord.Status = MinerStatus.LoginPage;

                    // 等待登录元素加载
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    IWebElement usernameElement = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[placeholder='Username or Email']")));
                    usernameElement.SendKeys(userName);

                    IWebElement passwordElement = driver.FindElement(By.CssSelector("input[placeholder='Password']"));
                    passwordElement.SendKeys(password);

                    IWebElement loginButton = driver.FindElement(By.CssSelector("button[type='submit']"));
                    loginButton.Click();

                    // 等待登入完成
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(text(), 'Refresh')]")));

                    driver.FindElement(By.XPath("//*[contains(text(), 'Refresh')]")).Click();

                    System.Threading.Thread.Sleep(20000);
                    _minerRecord.LoginUserName = userName;
                }
                catch (Exception ex)
                {
                    _minerRecord.Status = MinerStatus.LoginError;
                    _minerRecord.Exception = ex.ToString();
                    _minerRecord.ExceptionTime = DateTime.Now;
                    return;
                }


                driver.Navigate().GoToUrl("chrome-extension://ilehaonighjijnmpnagapkhpcdbhclfg/index.html");
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
            }
            finally
            {
                driver?.Quit();
                driver = null;
            }
        }

    }
}
