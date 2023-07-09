using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {

        private readonly string _host;
        private readonly string _port; 
        
        private ConnectionMultiplexer _redis;

        public IDatabase DataBase { get; set; }

        public RedisService(IConfiguration configuration)
        {
            _host = configuration["Redis:Host"];
            _port = configuration["Redis:Port"];
        }

        public void Connect()
        {
            var configString = $"{_host}:{_port}";

            _redis=ConnectionMultiplexer.Connect(configString);            
        }

        public IDatabase GetDatabase(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
