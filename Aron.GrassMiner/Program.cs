using GrassMiner.Data;
using GrassMiner.Models;
using GrassMiner.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SW.NetCore2.Extensions;
using System.Diagnostics;
using Quartz;
using Aron.GrassMiner.Jobs;
using NLog.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IMinerService minerService = null;

        try
        {
            var builder = WebApplication.CreateBuilder(args);

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
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<AppUser>(opt =>
            {
                //密碼長度
                opt.Password.RequiredLength = 4;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;

                opt.User.RequireUniqueEmail = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = false;
                opt.SignIn.RequireConfirmedAccount = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.SetDefaultJsonConvert();

            // copy db/TempGrass.db to db/Grass.db
            if (!Directory.Exists("db"))
            {
                Directory.CreateDirectory("db");
            }

            if (!File.Exists("db/Grass.db"))
            {
                File.Copy("Temp/TempGrass.db", "db/Grass.db");
            }
            AppConfig appConfig = new AppConfig();
            builder.Configuration.Bind("app", appConfig);
            if (Environment.GetEnvironmentVariables().Contains("GRASS_USER"))
            {
                appConfig.UserName = Environment.GetEnvironmentVariable("GRASS_USER").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("GRASS_PASS"))
            {
                appConfig.Password = Environment.GetEnvironmentVariable("GRASS_PASS").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("ADMIN_USER"))
            {
                appConfig.AdminUserName = Environment.GetEnvironmentVariable("ADMIN_USER").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("ADMIN_PASS"))
            {
                appConfig.AdminPassword = Environment.GetEnvironmentVariable("ADMIN_PASS").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("PROXY_ENABLE"))
            {
                appConfig.ProxyEnable = Environment.GetEnvironmentVariable("PROXY_ENABLE").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("PROXY_HOST"))
            {
                appConfig.ProxyHost = Environment.GetEnvironmentVariable("PROXY_HOST").ToString();
            }
            if (Environment.GetEnvironmentVariables().Contains("PROXY_USER"))
            {
                appConfig.ProxyUser = Environment.GetEnvironmentVariable("PROXY_USER").ToString();
            }

            if (Environment.GetEnvironmentVariables().Contains("PROXY_PASS"))
            {
                appConfig.ProxyUser = Environment.GetEnvironmentVariable("PROXY_PASS").ToString();
            }

            if (Environment.GetEnvironmentVariables().Contains("SHOW_CHROME"))
            {
                appConfig.ShowChrome = bool.Parse(Environment.GetEnvironmentVariable("SHOW_CHROME").ToString());
            }

            if (Environment.GetEnvironmentVariables().Contains("IS_COMMUNITY"))
            {
                appConfig.IsCommunity = bool.Parse(Environment.GetEnvironmentVariable("IS_COMMUNITY").ToString());
            }

            if (Environment.GetEnvironmentVariables().Contains("LOG_ENABLE"))
            {
                appConfig.LogEnable = bool.Parse(Environment.GetEnvironmentVariable("LOG_ENABLE").ToString());
            }

            builder.Services.AddSingleton(appConfig);

            if (appConfig.LogEnable)
            {
                builder = AddLog(builder);
            }

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            
            ApplicationDbContext context = new ApplicationDbContext(optionsBuilder.Options);


            try
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == appConfig.AdminUserName);
                UserManager<AppUser> userManager = new UserManager<AppUser>(new UserStore<AppUser>(context), null, new PasswordHasher<AppUser>(), null, null, null, null, null, null);
                if (user == null)
                {
                    AppUser identityUser = new AppUser();
                    identityUser.UserName = appConfig.AdminUserName;
                    identityUser.EmailConfirmed = true;
                    await userManager.CreateAsync(identityUser, appConfig.AdminPassword);
                    context.SaveChanges();

                }
                else
                {
                    if (user.PasswordHash != new PasswordHasher<AppUser>().HashPassword(user, appConfig.AdminPassword))
                    {
                        user.PasswordHash = new PasswordHasher<AppUser>().HashPassword(user, appConfig.AdminPassword);
                        await userManager.UpdateAsync(user);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            context.Dispose();




            MinerRecord minerRecord = new MinerRecord();
            builder.Services.AddSingleton(minerRecord);

            if (appConfig.IsCommunity)
            {
                minerService = new CommunityMinerService(appConfig, minerRecord);
            }
            else
            {
                minerService = new MinerService(appConfig, minerRecord);
            }
            builder.Services.AddSingleton(minerService);

            builder.Services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();



                //建立 job
                var jobKey = new JobKey("UpdateIpJob");
                q.AddJob<UpdateJob>(jobKey);
                //建立 trigger(規則) 來觸發 job
                q.AddTrigger(t => t
                    .WithIdentity("UpdateIpJob")
                    .ForJob(jobKey)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(10)
                        .RepeatForever())
                );

                var jobKey2 = new JobKey("ScreensShotJob");
                q.AddJob<ScreensShotJob>(jobKey2);
                q.AddTrigger(t => t
                       .WithIdentity("ScreensShotJob")
                              .ForJob(jobKey2)
                                     .StartNow()
                                            .WithSimpleSchedule(x => x
                                                       .WithIntervalInSeconds(5)
                                                                  .RepeatForever())
                                               );

                var jobKey3 = new JobKey("ExitJob");
                q.AddJob<ExitJob>(jobKey3);
                q.AddTrigger(t => t
                    .WithIdentity("ExitJob")
                    .ForJob(jobKey3)
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(2)
                        .RepeatForever())
                );

            });

            builder.Services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            builder.Host.UseWindowsService(options =>
            {
                options.ServiceName = "Aron Grass Miner";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
                endpoints.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

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

    public static WebApplicationBuilder AddLog(WebApplicationBuilder builder)
    {
        var storage = new { Path = builder.Environment.ContentRootPath };


        var nlogConf = File.ReadAllText(Path.Combine(builder.Environment.ContentRootPath, "NLog_Template.config"));

        string basedir = Path.Combine(storage.Path).Replace("\\", "/");
        if (basedir.EndsWith("/"))
            basedir = basedir.Substring(0, basedir.Length - 1);
        nlogConf = nlogConf.Replace("${basedir}", basedir);

        File.WriteAllText(Path.Combine(builder.Environment.ContentRootPath, "NLog.config"), nlogConf);

        if (!Directory.Exists(Path.Combine(storage.Path, "logs")))
            Directory.CreateDirectory(Path.Combine(storage.Path, "logs"));

        builder
            .Logging
            .ClearProviders()
            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information)
            .AddNLog(Path.Combine(builder.Environment.ContentRootPath, "NLog.config"));

        return builder;

    }
}