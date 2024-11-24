using Aron.GrassMiner.Helpers;
using Aron.GrassMiner.Models;
using Aron.GrassMiner.ViewModels;
using Aron.NetCore.Util.Helpers;
using Aron.NetCore.Util.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Aron.GrassMiner.Services.Identity
{
    public class IdentityService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<IdentityService> _logger;
        private readonly JwtHelpers _jwtHelpers;
        private readonly TokenService _tokenService;
        private readonly ApiHelper _apiHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan expire = TimeSpan.FromDays(3);

        public IdentityService(AppConfig _appConfig, ILogger<IdentityService> logger, JwtHelpers jwtHelpers, TokenService tokenService, ApiHelper apiHelper, IHttpContextAccessor httpContextAccessor)
        {
            this._appConfig = _appConfig;
            this._logger = logger;
            this._jwtHelpers = jwtHelpers;
            this._tokenService = tokenService;
            this._apiHelper = apiHelper;
            this._httpContextAccessor = httpContextAccessor;
        }

        public ResponseResult<LoginResp> Login(RequestResult<LoginReq> value)
        {
            try
            {

                if(value.resultObj.Username == _appConfig.AdminUserName && value.resultObj.Password == _appConfig.AdminPassword)
                {
                    var roles = new List<string> { "Admin" };
                    string token = _jwtHelpers.GenerateToken(value.resultObj.Username, roles, expire);
                    _tokenService.StoreToken(token, expire);

                    LoginResp ret = new()
                    {
                        token_type = "bearer",
                        access_token = token,
                        expires_in = (int)expire.TotalSeconds,
                        issued = DateTime.UtcNow,
                        expires = DateTime.UtcNow + expire,
                        username = value.resultObj.Username,
                        roles = roles
                    };

                    return _apiHelper.CreateResponse(value.lang, ret);
                }
                else
                {
                    return _apiHelper.CreateErrorResponse<LoginResp>(value.lang, message: "Invalid username or password", null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Login", ex);

                return _apiHelper.CreateErrorResponse<LoginResp>(value.lang, message: "error " + ex, null);


            }

        }

        // logout
        public ResponseResult<string> Logout()
        {
            try
            {
                // get token from header
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
                if(!string.IsNullOrEmpty(token))
                {
                    _tokenService.RemoveToken(token);
                }
                return _apiHelper.CreateResponse<string>("", "Logout success");
            }
            catch (Exception ex)
            {
                _logger.LogError("Logout", ex);

                return _apiHelper.CreateErrorResponse<string>("", message: "error " + ex, null);
            }
        }

    }
}
