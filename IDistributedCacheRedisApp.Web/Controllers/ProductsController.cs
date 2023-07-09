using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "Taylan", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 200 };

            string jsonProduct = JsonSerializer.Serialize(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct, cacheEntryOptions);
            //await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            //string name= _distributedCache.GetString("name");
            //ViewBag.Name = name;

            //string jsonProduct = _distributedCache.GetString("product:1");

            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonProduct = Encoding.UTF8.GetString(byteProduct);


            ViewBag.Product = jsonProduct;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/indir.jpg");

            byte[] imagesByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imagesByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imagesByte = _distributedCache.Get("image");

           return File(imagesByte,"image/jpeg");
        }
    }
}
