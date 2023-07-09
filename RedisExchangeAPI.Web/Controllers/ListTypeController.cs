using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        private string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;

            _database = _redisService.GetDatabase(1);
        }


        public IActionResult Index()
        {
            List<string> nameList = new List<string>();

            if(_database.KeyExists(listKey))
            {
                _database.ListRange(listKey).ToList().ForEach(x=>
                {
                    nameList.Add(x);
                });
            }
            
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add (string name)
        {
            _database.ListRightPush(listKey,name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _database.ListRemove(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            _database.ListLeftPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
