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

// start vpnclient
try
{
    Process process = new Process()
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "/vpnclient/vpnclient",
            Arguments = "start",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };
    process.Start();
}
catch (Exception ex)
{
}

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

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
builder.Services.AddSingleton(appConfig);

DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
optionsBuilder.LogTo(s => Debug.WriteLine(s));
optionsBuilder.EnableSensitiveDataLogging();
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

MinerService minerService = new MinerService(appConfig, minerRecord);
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

    //建立 job
    var jobKey2 = new JobKey("VPNJob");
    q.AddJob<VPNJob>(jobKey2);
    //建立 trigger(規則) 來觸發 job
    q.AddTrigger(t => t
           .WithIdentity("VPNJob")
           .ForJob(jobKey2)
           .StartNow()
           .WithSimpleSchedule(x => x
           .WithIntervalInMinutes(60)
                .RepeatForever())
    );
});

builder.Services.AddQuartzHostedService(opt =>
{
    opt.WaitForJobsToComplete = true;
});
builder.Services.AddMySwagger();


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
app.UseMySwagger();

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

minerService.driver?.Quit();
minerService.driver?.Dispose();

