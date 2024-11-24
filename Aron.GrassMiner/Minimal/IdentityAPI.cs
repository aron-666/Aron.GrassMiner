using Aron.GrassMiner.Models;
using Aron.GrassMiner.Services;
using Aron.GrassMiner.Services.Identity;
using Aron.GrassMiner.ViewModels;
using Aron.NetCore.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Aron.GrassMiner.Minimal
{
    public static class IdentityAPI
    {
        public static WebApplication AddIdentityAPI(this WebApplication app)
        {

            app.MapPost("/api/Identity/Login", async (HttpContext httpContext, IdentityService identityService) =>
            {
                var loginReq = await httpContext.Request.ReadFromJsonAsync<RequestResult<LoginReq>>(MyJsonContext.Default.RequestResultLoginReq.Options);
                var ret = identityService.Login(loginReq);
                var options = MyJsonContext.Default.ResponseResultLoginResp.Options;
                return Results.Json(ret, options);
            });


            app.MapDelete("/api/Identity/Logout", (IdentityService identityService) =>
            {
                var ret = identityService.Logout();
                var options = MyJsonContext.Default.ResponseResultString.Options;
                return Results.Json(ret, options);
            });

            return app;
        }
    }
}
