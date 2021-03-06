using System;
using System.Threading.Tasks;
using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Models;
using Microsoft.AspNetCore.Http;

namespace TTCore.StoreProvider.Middleware
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly DbMemoryContext _db;
        private readonly DbUserContext _dbUser;

        public FactoryActivatedMiddleware(DbMemoryContext db, DbUserContext dbUser)
        {
            _db = db;
            _dbUser = dbUser;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var keyValue = context.Request.Query["key"];
            //if (!string.IsNullOrWhiteSpace(keyValue))
            {
                //_db.Add(new Request()
                //    {
                //        DT = DateTime.UtcNow, 
                //        MiddlewareActivation = "FactoryActivatedMiddleware", 
                //        Value = Guid.NewGuid().ToString()
                //    });

                //await _db.SaveChangesAsync();
            }

            await next(context);
        }
    }
}
