using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace TTCore.StoreProvider.Controllers
{
    public class CacheMemoryRuntimeController : Controller
    {
        private IMemoryCache _cache;

        public CacheMemoryRuntimeController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public IActionResult Index()
        {
            return RedirectToAction("CacheGet");
        }

        #region snippet1
        public IActionResult CacheTryGetValueSet()
        {
            DateTime cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }

            return View("Cache", cacheEntry);
        }
        #endregion

        #region snippet_gct
        public IActionResult CacheGet()
        {
            var cacheEntry = _cache.Get<DateTime?>(CacheKeys.Entry);
            return View("Cache", cacheEntry);
        }
        #endregion

        #region snippet2
        public IActionResult CacheGetOrCreate()
        {
            var cacheEntry = _cache.GetOrCreate(CacheKeys.Entry, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return DateTime.Now;
            });

            return View("Cache", cacheEntry);
        }

        public async Task<IActionResult> CacheGetOrCreateAsync()
        {
            var cacheEntry = await
                _cache.GetOrCreateAsync(CacheKeys.Entry, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                return Task.FromResult(DateTime.Now);
            });

            return View("Cache", cacheEntry);
        }
        #endregion

        public IActionResult CacheRemove()
        {
            _cache.Remove(CacheKeys.Entry);
            return RedirectToAction("CacheGet");
        }

        #region snippet_et
        public IActionResult CreateCallbackEntry()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Pin to cache.
                .SetPriority(CacheItemPriority.NeverRemove)
                // Add eviction callback
                .RegisterPostEvictionCallback(callback: EvictionCallback, state: this);

            _cache.Set(CacheKeys.CallbackEntry, DateTime.Now, cacheEntryOptions);

            return RedirectToAction("GetCallbackEntry");
        }

        public IActionResult GetCallbackEntry()
        {
            return View("Callback", new CallbackViewModel
            {
                CachedTime = _cache.Get<DateTime?>(CacheKeys.CallbackEntry),
                Message = _cache.Get<string>(CacheKeys.CallbackMessage)
            });
        }

        public IActionResult RemoveCallbackEntry()
        {
            _cache.Remove(CacheKeys.CallbackEntry);
            return RedirectToAction("GetCallbackEntry");
        }

        private static void EvictionCallback(object key, object value,
            EvictionReason reason, object state)
        {
            var message = $"Entry was evicted. Reason: {reason}.";
            ((CacheMemoryRuntimeController)state)._cache.Set(CacheKeys.CallbackMessage, message);
        }
        #endregion

        #region snippet_ed
        public IActionResult CreateDependentEntries()
        {
            var cts = new CancellationTokenSource();
            _cache.Set(CacheKeys.DependentCTS, cts);

            using (var entry = _cache.CreateEntry(CacheKeys.Parent))
            {
                // expire this entry if the dependant entry expires.
                entry.Value = DateTime.Now;
                entry.RegisterPostEvictionCallback(DependentEvictionCallback, this);

                _cache.Set(CacheKeys.Child,
                    DateTime.Now,
                    new CancellationChangeToken(cts.Token));
            }

            return RedirectToAction("GetDependentEntries");
        }

        public IActionResult GetDependentEntries()
        {
            return View("Dependent", new DependentViewModel
            {
                ParentCachedTime = _cache.Get<DateTime?>(CacheKeys.Parent),
                ChildCachedTime = _cache.Get<DateTime?>(CacheKeys.Child),
                Message = _cache.Get<string>(CacheKeys.DependentMessage)
            });
        }

        public IActionResult RemoveChildEntry()
        {
            _cache.Get<CancellationTokenSource>(CacheKeys.DependentCTS).Cancel();
            return RedirectToAction("GetDependentEntries");
        }

        private static void DependentEvictionCallback(object key, object value,
            EvictionReason reason, object state)
        {
            var message = $"Parent entry was evicted. Reason: {reason}.";
            ((CacheMemoryRuntimeController)state)._cache.Set(CacheKeys.DependentMessage, message);
        }
        #endregion

        #region snippet_cancel
        public IActionResult CancelTest()
        {
            var cachedVal = DateTime.Now.Second.ToString();
            CancellationTokenSource cts = new CancellationTokenSource();
            _cache.Set<CancellationTokenSource>(CacheKeys.CancelTokenSource, cts);

            // Don't use previous message.
            _cache.Remove(CacheKeys.CancelMsg);

            _cache.Set(CacheKeys.Ticks, cachedVal,
                new MemoryCacheEntryOptions()
                .AddExpirationToken(new CancellationChangeToken(cts.Token))
                .RegisterPostEvictionCallback(
                    (key, value, reason, substate) =>
                    {
                        var cm = $"'{key}':'{value}' was evicted because: {reason}";
                        _cache.Set<string>(CacheKeys.CancelMsg, cm);
                    }
                ));

            return RedirectToAction("CheckCancel");
        }

        public IActionResult CheckCancel(int? id = 0)
        {
            if (id > 0)
            {
                CancellationTokenSource cts =
                   _cache.Get<CancellationTokenSource>(CacheKeys.CancelTokenSource);
                cts.CancelAfter(100);
                // Cancel immediately with cts.Cancel();
            }

            ViewData["CachedTime"] = _cache.Get<string>(CacheKeys.Ticks);
            ViewData["Message"] = _cache.Get<string>(CacheKeys.CancelMsg); ;

            return View();
        }
        #endregion

    }

    public static class CacheKeys
    {
        public static string Entry { get { return "_Entry"; } }
        public static string CallbackEntry { get { return "_Callback"; } }
        public static string CallbackMessage { get { return "_CallbackMessage"; } }
        public static string Parent { get { return "_Parent"; } }
        public static string Child { get { return "_Child"; } }
        public static string DependentMessage { get { return "_DependentMessage"; } }
        public static string DependentCTS { get { return "_DependentCTS"; } }
        public static string Ticks { get { return "_Ticks"; } }
        public static string CancelMsg { get { return "_CancelMsg"; } }
        public static string CancelTokenSource { get { return "_CancelTokenSource"; } }
    }
    public class CallbackViewModel
    {
        public DateTime? CachedTime { get; set; }
        public string Message { get; set; }
    }
    public class DependentViewModel
    {
        public DateTime? ParentCachedTime { get; set; }
        public DateTime? ChildCachedTime { get; set; }
        public string Message { get; set; }
    }
}