using System.Text.Json;
using Aron.GrassMiner.Models;
using System.Text.Json.Serialization.Metadata;
using NLog.Extensions.Logging;
using Aron.GrassMiner.Data;
using Microsoft.AspNetCore.Identity;
using Aron.GrassMiner.Jobs;
using Aron.GrassMiner.Services;
using Aron.GrassMiner.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using Aron.GrassMiner.Services.Identity;
using System.IdentityModel.Tokens.Jwt;
using Aron.NetCore.Util.Extensions;

namespace Aron.GrassMiner.Extensions
{
    public static class ServiceExtrensions
    {

        public static IServiceCollection AddEnv(this IServiceCollection services, AppConfig appConfig)
        {
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

            if (Environment.GetEnvironmentVariables().Contains("LOG_ENABLE"))
            {
                appConfig.LogEnable = bool.Parse(Environment.GetEnvironmentVariable("LOG_ENABLE").ToString());
            }
            return services;
        }



        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IdentityService>();
            services.AddSingleton<TokenService>();
            services.AddMemoryCache();
            services.AddApiHelpers();




            return services;
        }


        public static IServiceCollection AddJobs(this IServiceCollection services)
        {
            services.AddHostedService<UpdateJob>();
            services.AddHostedService<ScreensShotJob>();

            return services;
        }


        public static WebApplicationBuilder AddLog(this WebApplicationBuilder builder)
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

        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            // 隨機生成 SignKey
            byte[] key = new byte[32]; // 256 位元的金鑰
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            string signKey = Convert.ToBase64String(key);

            JwtSettings settings = new JwtSettings()
            {
                Issuer = "Aron666",
                SignKey = signKey
            };

            services.AddSingleton(settings);
            services.AddSingleton<JwtHelpers>();
            services.AddSingleton<TokenService>(); // 註冊 TokenService


            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SignKey))
                    };

                    // 在 Token 驗證成功後，檢查 Token 是否已經被儲存
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var tokenService = context.HttpContext.RequestServices.GetRequiredService<TokenService>();
                            var token = context.SecurityToken as JwtSecurityToken;
                            if (token != null && !tokenService.IsTokenValid(token.RawData))
                            {
                                context.Fail("Token is not valid.");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddHttpContextAccessor();
            return services;
        }

        
    }

    
}
