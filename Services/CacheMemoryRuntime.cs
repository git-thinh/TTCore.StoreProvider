using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.Services
{
    public class CacheMemoryRuntime
    {
        public MemoryCache Cache { get; private set; }
        public CacheMemoryRuntime()
        {
            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024
            });
        }
    }
}
