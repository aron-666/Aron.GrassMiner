using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aron.GrassMiner.WinControl
{
    public partial class ConfigForm : Form
    {
        string appsetting = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {

            LoadConfig();
            ckProxy_CheckedChanged(sender, e);
            ckProxyAuth_CheckedChanged(sender, e);
        }


        private void LoadConfig()
        {
            // Load config from file
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(appsetting, true, true)
                .Build();

            string url = config["Kestrel:EndPoints:Http:Url"];
            txUrl.Text = url;


            AppConfig appConfig = new AppConfig();
            config.GetSection("app").Bind(appConfig);

            // Set values to controls
            txGrassUser.Text = appConfig.UserName;
            txGrassPass.Text = appConfig.Password;
            txAdminUser.Text = appConfig.AdminUserName;
            txAdminPass.Text = appConfig.AdminPassword;
            ckProxy.Checked = appConfig.ProxyEnable == "true";
            txProxyHost.Text = appConfig.ProxyHost;
            ckProxyAuth.Checked = !string.IsNullOrEmpty(appConfig.ProxyUser);
            txProxyUser.Text = appConfig.ProxyUser;
            txProxyPass.Text = appConfig.ProxyPass;
        }

        private void ckProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (ckProxy.Checked)
            {
                panelProxy.Enabled = true;
            }
            else
            {
                panelProxy.Enabled = false;
            }
        }

        private void ckProxyAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (ckProxyAuth.Checked)
            {
                panelAuth.Enabled = true;
            }
            else
            {
                panelAuth.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(appsetting, true, true)
                .Build();



                Kestrel kestrel = new Kestrel();
                config.GetSection("Kestrel").Bind(kestrel);
                kestrel.EndPoints.Http.Url = txUrl.Text;


                AppConfig appConfig = new AppConfig();
                config.GetSection("app").Bind(appConfig);
                appConfig.UserName = txGrassUser.Text;
                appConfig.Password = txGrassPass.Text;
                appConfig.AdminUserName = txAdminUser.Text;
                appConfig.AdminPassword = txAdminPass.Text;
                appConfig.ProxyEnable = ckProxy.Checked ? "true" : "false";
                appConfig.ProxyHost = txProxyHost.Text;
                appConfig.ProxyUser = ckProxyAuth.Checked ? txProxyUser.Text : "";
                appConfig.ProxyPass = ckProxyAuth.Checked ? txProxyPass.Text : "";
                // Save config to file

                string json = JsonConvert.SerializeObject(new { Kestrel = kestrel, app = appConfig }, Formatting.Indented);
                File.WriteAllText(appsetting, json);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
