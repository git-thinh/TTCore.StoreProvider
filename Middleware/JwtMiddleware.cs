using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using TTCore.StoreProvider.Models;
using TTCore.StoreProvider.Services;

namespace TTCore.StoreProvider.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { Ok = false, Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IJwtService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            try
            {
                var jwtToken = userService.ValidateToken(token);
                if (jwtToken != null)
                {
                    var userName = jwtToken.Claims.First(x => x.Type == "id").Value;
                    var user = userService.GetByUsername(userName);
                    // attach user to context on successful jwt validation
                    context.Items["User"] = user;
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }

            await _next(context);
        }
    }
}
