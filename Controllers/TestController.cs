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

namespace Mascot.SharePoint.Service
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        readonly AppDbContext _db;
        readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger,
            AppDbContext db)
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
    }
}
