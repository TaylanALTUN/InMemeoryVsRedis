using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;

            _database = _redisService.GetDatabase(0);
        }

        public IActionResult Index()
        {
            _database.StringSet("name", "Taylan ALTUN");
            _database.StringSet("ziyaretci", 100);
            return View();
        }

        public IActionResult Show()
        {
            var value = _database.StringGet("name");

            var vizitor= _database.StringIncrement("ziyaretci", 1);

            ViewBag.vizitor = vizitor;

            if (value.HasValue)
            {
                ViewBag.value = value;
            }
            return View();
        }
    }
}
