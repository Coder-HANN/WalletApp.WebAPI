//using StackExchange.Redis;
//using WalletApp.Infrastructure.Services.MemoryCach;
//using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

//namespace WalletApp.Application.Abstraction.Services.Redis
//{
//    public class RedisCacheManager : ICacheManager
//    {
//        private readonly IDatabase _database;
//        private readonly ConnectionMultiplexer _redis;

//        public RedisCacheManager(IConfiguration configuration)
//        {
//            var connection = configuration["RedisSettings:ConnectionString"];
//            _redis = ConnectionMultiplexer.Connect(connection);
//            _database = _redis.GetDatabase();
//        }
//        public void Add(string key, object data, int duration) 
//        {
//            if (data != null)
//            {
//                var jsonData = System.Text.Json.JsonSerializer.Serialize(data);
//                _database.StringSet(key, jsonData, TimeSpan.FromMinutes(duration));
//            }
//        }

//        public T Get<T>(string key)
//        {
//            var value = _database.StringGet(key);
//            if (!value.HasValue) return default(T);

//            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
//        }

//        public object Get(string key)
//        {
//            var value = _database.StringGet(key);
//            return value.HasValue ? (object)value.ToString() : null;
//        }

//        public bool IsAdd(string key)
//        {
//            return _database.KeyExists(key);
//        }

//        public void Remove(string key)
//        {
//            _database.KeyDelete(key);
//        }

//        public void RemoveByPattern(string pattern)
//        {
//            var endpoints = _redis.GetEndPoints();
//            foreach (var endpoint in endpoints)
//            {
//                var server = _redis.GetServer(endpoint);
//                foreach (var key in server.Keys(pattern: $"*{pattern}*"))
//                {
//                    _database.KeyDelete(key);
//                }
//            }
//        }
//    }
//}
