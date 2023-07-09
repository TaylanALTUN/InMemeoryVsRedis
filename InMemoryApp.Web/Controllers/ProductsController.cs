using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // 1. yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}

            // 2.yol
            //if (!_memoryCache.TryGetValue("time", out string timeCache))
            //{ 
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}


            // 3.yol
            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{
            //    //entry.SlidingExpiration = TimeSpan.FromSeconds(1);  
            //    return DateTime.Now.ToString();
            //});

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority=CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"key: {key}, value: {value}, reason: {reason}, state: {state}");
            });

            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

            Product product = new Product { Id=1,Name= "Kalem", Price=200 };

            _memoryCache.Set<Product>("product:1", product);

            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.Remove("time");
            _memoryCache.TryGetValue("time", out string timeCache);

            _memoryCache.TryGetValue("callback", out string callbackCache);

            ViewBag.time = timeCache;
            ViewBag.callback = callbackCache;
            ViewBag.product = _memoryCache.Get<Product>("product:1");
            return View();
        }
    }
}
