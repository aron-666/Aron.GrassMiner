using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Aron.GrassMiner.WinControl
{
    public partial class Form1 : Form
    {
        string appsetting = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
        string exe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app", "Aron.GrassMiner.exe");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private bool IsAppRunning(out Process? prc)
        {
            try
            {
                //檢查是否正在執行
                prc = Process.GetProcesses()
                    .FirstOrDefault(p =>
                    {
                        try
                        {
                            return string.Equals(p.MainModule.FileName, exe, StringComparison.OrdinalIgnoreCase);

                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    });
                return prc != null;
            }
            catch (Exception ex)
            {
                prc = null;
                MessageBox.Show(ex.ToString());
                return false;
            }

        }

        private void ckServiceInstalled_CheckedChanged(object sender, EventArgs e)
        {
            if (ckServiceInstalled.Checked)
            {
                ckServiceInstalled.BackColor = Color.Blue;
            }
            else
            {
                ckServiceInstalled.BackColor = Color.White;
            }
        }

        private void ckServiceInstalled_Click(object sender, EventArgs e)
        {
            if(IsAppRunning(out var prc))
            {
                MessageBox.Show("Please stop the app first.");
                return;
            }
            if (!ckServiceInstalled.Checked)
            {
                
                UninstallService();
            }
            else
            {
                InstallService();
            }

        }

        private bool IsInstalledService()
        {
            try
            {
                // sc query GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"query GrassMiner";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                return output.Contains("SERVICE_NAME: GrassMiner");
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        private void InstallService()
        {
            try
            {
                //install service
                //sc create GrassMiner binPath= "C:\Users\Aron\source\repos\Aron.GrassMiner\Aron.GrassMiner\bin\Debug\net5.0\Aron.GrassMiner.exe"
                //sc start GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"create GrassMiner binPath= \"{exe}\" start=auto";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                process.Close();

                // description
                // sc description GrassMiner "Grass Miner Service"
                startInfo.Arguments = $"description GrassMiner \"Grass Miner Service\"";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void StartService()
        {
            try
            {
                //start service
                //sc start GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"start GrassMiner";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void StopService()
        {
            try
            {
                //stop service
                //sc stop GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"stop GrassMiner";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void UninstallService()
        {
            try
            {
                //uninstall service
                //sc stop GrassMiner
                //sc delete GrassMiner
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "sc";
                startInfo.Arguments = $"stop GrassMiner";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                process.Close();

                startInfo.Arguments = $"delete GrassMiner";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            try
            {
                //檢查是否正在執行
                if (IsAppRunning(out var prc))
                {
                    // 傳送中止指令
                    File.Create(Path.Combine(app, "shutdown.flag")).Close();
                    return;
                }
                //檢查是否安裝Service
                if (IsInstalledService())
                {
                    //啟動Service
                    StartService();
                    return;
                }



                //run dotnet app\Aron.GrassMiner.dll
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = app;
                startInfo.FileName = exe;
                startInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = appsetting;

                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            try
            {
                //open file explorer to app\\logs

                string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
                string logs = Path.Combine(app, "logs");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "explorer.exe";
                startInfo.Arguments = logs;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = false;
                startInfo.CreateNoWindow = false;
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsAppRunning(out var prc))
                {
                    btnControl.Text = "Stop";
                    lbModule.BackColor = Color.Green;
                    lbPid.Text = prc?.Id.ToString();

                }
                else
                {
                    btnControl.Text = "Start";
                    lbModule.BackColor = Color.Transparent;
                    lbPid.Text = "";
                }

                if (IsInstalledService())
                {
                    ckServiceInstalled.Checked = true;
                }
                else
                {
                    ckServiceInstalled.Checked = false;
                }
            }
            catch (Exception ex)
            {
            }




        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsAppRunning(out var prc))
                {
                    IConfiguration Configuration = new ConfigurationBuilder()
                        .AddJsonFile(appsetting, optional: true, reloadOnChange: true)
                        .Build();

                    //Kestrel.EndPoints.Http.Url
                    string url = Configuration["Kestrel:EndPoints:Http:Url"];
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = url;
                    startInfo.UseShellExecute = true;
                    startInfo.RedirectStandardOutput = false;
                    startInfo.RedirectStandardError = false;
                    startInfo.CreateNoWindow = false;
                    Process process = new Process();
                    process.StartInfo = startInfo;
                    process.Start();

                }
                else
                {
                    MessageBox.Show("Please start the app first.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();

        }
    }
}


