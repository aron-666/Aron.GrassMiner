using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using NLog.Extensions.Logging;
using Aron.GrassMiner.Models;
using Aron.GrassMiner.Jobs;
using Aron.GrassMiner.Services;
using Aron.GrassMiner.Extensions;
using Aron.GrassMiner.Data;
using Aron.GrassMiner.Minimal;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IMinerService minerService = null;

        try
        {
            var builder = WebApplication.CreateSlimBuilder(args);

            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.System);

            //判斷是否是Winodws service目錄
            if (Directory.GetCurrentDirectory().ToLowerInvariant() == winDir.ToLowerInvariant())
            {
                //取得exe full path
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                //取得exe所在目錄
                string exeDir = Path.GetDirectoryName(exePath);
                builder.Configuration.SetBasePath(exeDir);
                Directory.SetCurrentDirectory(exeDir);
            }
            else
            {
                builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            }

            if (File.Exists("../appsettings.json"))
            {
                //取得上級目錄的appsettings.json
                string path = Path.Combine(Directory.GetCurrentDirectory(), "../appsettings.json");
                builder.Configuration.AddJsonFile(path, optional: false, reloadOnChange: true);
            }
            builder.Configuration
                .AddEnvironmentVariables();

            // Add services to the container.



            AppConfig appConfig = new AppConfig();
            builder.Configuration.Bind("app", appConfig);
            builder.Services.AddSingleton(appConfig);

            builder
                .Services
                .AddEnv(appConfig)
                .AddApplication()
                .AddJwt(builder.Configuration)
                .AddJobs();


            if (appConfig.LogEnable)
            {
                builder.AddLog();
            }

            MinerRecord minerRecord = new MinerRecord();
            builder.Services.AddSingleton(minerRecord);
            minerService = new MinerService(appConfig, minerRecord);
            builder.Services.AddSingleton(minerService);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMinerAPI();
            app.AddIdentityAPI();
            app.UseIndex();
            



            app.Run();
        }
        finally
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            NLog.LogManager.Shutdown();
            minerService?.driver?.Close();
            minerService?.driver?.Quit();
            minerService?.driver?.Dispose();
        }
    }
}