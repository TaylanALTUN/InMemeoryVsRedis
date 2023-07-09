using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;

        private string listKey = "setnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;

            _database = _redisService.GetDatabase(2);
        }
        public IActionResult Index()
        {

            HashSet<string> nameList = new HashSet<string>();

            if(_database.KeyExists(listKey))
            {
                _database.SetMembers(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });

            }
            return View(nameList);
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            _database.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _database.SetRemove(listKey, name);
            return RedirectToAction("Index");
        }


    }
}
