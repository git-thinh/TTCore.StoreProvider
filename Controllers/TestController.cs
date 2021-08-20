using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Docs.Samples;

namespace TTCore.StoreProvider.Apis
{
    [ApiRouteBase]
    public class TestController : ControllerBase
    {
        readonly DbMemoryContext _db;
        readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger,
            DbMemoryContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<IEnumerable<Request>> Get()
        {
            //var site = this.getSiteSetting();
            List<Request> ls = await _db.Requests.ToListAsync();
            return ls;
        }

        [HttpGet("search/{keyword}")]
        public async Task<IEnumerable<Request>> FindByValue(string keyword)
        {
            List<Request> ls = await _db.Requests.Where(x => x.Value.Contains(keyword)).ToListAsync();
            return ls;
        }

        [HttpGet("search/{id:int}")]
        public async Task<IEnumerable<Request>> FindById(int id)
        {
            List<Request> ls = await _db.Requests.Where(x => x.Id == id).ToListAsync();
            return ls;
        }

        [HttpGet("[action]")]
        public async Task<Request> CreateRadomItem()
        {
            var item = new Request()
            {
                Id = _db.Requests.Count() + 1,
                DT = DateTime.UtcNow,
                MiddlewareActivation = "FactoryActivatedMiddleware",
                Value = Guid.NewGuid().ToString()
            };
            _db.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        [HttpGet("{id:customName}")]
        public IActionResult Get(string id)
        {
            return ControllerContext.MyDisplayRouteInfo(id);
        }

        [HttpGet("my/{id:customName}")]
        public IActionResult Get(int id)
        {
            return ControllerContext.MyDisplayRouteInfo(id);
        }

    }
}
