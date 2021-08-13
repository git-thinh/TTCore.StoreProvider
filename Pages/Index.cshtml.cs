using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TTCore.StoreProvider.Data;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DbMemoryContext _db;

        public IndexModel(DbMemoryContext db)
        {
            _db = db;
        }

        public List<Request> Requests { get; private set; }

        public async Task OnGetAsync()
        {
            Requests = await _db.Requests.ToListAsync();
        }
    }
}
