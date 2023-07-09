using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        private string listKey = "dictionary";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;

            _database = _redisService.GetDatabase(3);
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            if (_database.KeyExists(listKey))
            {
                
                _database.HashGetAll(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });

            }
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            _database.HashSet(listKey, name, value);
            _database.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string key)
        {
            _database.HashDelete(listKey, key);
            return RedirectToAction("Index");
        }
    }
}
