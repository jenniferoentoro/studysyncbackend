using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using source_service.Service.Interface;
using System.Text.Json;

namespace source_service.Service
{

    public class CacheServise : ICacheService
    {

        private IDatabase _cacheDb;
        public CacheServise()
        {
            var redis = ConnectionMultiplexer.Connect("redis-cache:6379");
            _cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default(T);
        }

        public object RemoveData(string key)
        {
            var _exist = _cacheDb.KeyExists(key);
            if (_exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        }
        public void RemoveBySubstring(string substring)
        {
            var server = _cacheDb.Multiplexer.GetServer("redis-cache:6379");

            foreach (var key in server.Keys())
            {
                if (key.ToString().Contains(substring))
                {
                    _cacheDb.KeyDelete(key);
                }
            }
        }

        // fine all cache key
        public List<string> GetAllKeys()
        {
            var server = _cacheDb.Multiplexer.GetServer("redis-cache:6379");
            List<string> keys = new List<string>();
            foreach (var key in server.Keys())
            {
                keys.Add(key);
            }
            return keys;
        }
    }
    //     public T GetData<T>(string key)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public object RemoveData(string key)
    //     {
    //         throw new NotImplementedException();
    //     }

    //     public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    //     {
    //         throw new NotImplementedException();
    //     }
    // }
}