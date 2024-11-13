using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Service.Interface
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        object RemoveData(string key);
        void RemoveBySubstring(string substring);
        List<string> GetAllKeys();
    }
}