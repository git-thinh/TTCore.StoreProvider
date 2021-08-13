using System;
using System.Threading.Tasks;
using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Models;
using Microsoft.AspNetCore.Http;

namespace TTCore.StoreProvider.Middleware
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly AppDbContext _db;

        public FactoryActivatedMiddleware(AppDbContext db)
        {
            _db = db;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var keyValue = context.Request.Query["key"];
            //if (!string.IsNullOrWhiteSpace(keyValue))
            {
                _db.Add(new Request()
                    {
                        DT = DateTime.UtcNow, 
                        MiddlewareActivation = "FactoryActivatedMiddleware", 
                        Value = Guid.NewGuid().ToString()
                    });

                await _db.SaveChangesAsync();
            }

            await next(context);
        }
    }
}
