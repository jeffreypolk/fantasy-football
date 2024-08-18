using Microsoft.Extensions.Caching.Distributed;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Text.Json;

namespace FF.Backend.Caching
{
    public class CacheService<T> where T : class
    {
        private readonly IDistributedCache _cache;
        private readonly string _storeName;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            _storeName = typeof(T).FullName;
        }

        public T2 Get<T2>(string key) where T2 : class
        {
            var keyToUse = GetFullKeyName(key);
            var json = _cache.GetString(keyToUse);
            if (string.IsNullOrEmpty(json)) 
            { 
                return null; 
            }
            var obj = JsonSerializer.Deserialize<T2>(json);
            return obj;
        }

        public async Task<T2> GetAsync<T2>(string key) where T2 : class
        {
            var keyToUse = GetFullKeyName(key);
            var json = await _cache.GetStringAsync(keyToUse);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var obj = JsonSerializer.Deserialize<T2>(json);
            return obj;
        }

        public void Set<T2>(string key, T2 obj)
        {
            var keyToUse = GetFullKeyName(key);
            var json = JsonSerializer.Serialize(obj);
            _cache.SetString(keyToUse, json);
        }

        public async Task SetAsync<T2>(string key, T2 obj)
        {
            var keyToUse = GetFullKeyName(key);
            var json = JsonSerializer.Serialize(obj);
            await _cache.SetStringAsync(keyToUse, json);
        }

        private string GetFullKeyName(string key)
        {
            return $"{_storeName}_{key}";
        }
    }


    //public T Get<T>(string key) where T : class
    //{
    //    var bytes = _cache.Get(key);
    //    if (bytes == null)
    //    {
    //        return default(T);
    //    }
    //    return FromByteArray<T>(bytes);
    //}

    //public async Task<T> GetAsync<T>(string key) where T : class
    //{
    //    var bytes = await _cache.GetAsync(key);
    //    if (bytes == null)
    //    {
    //        return default(T);
    //    }
    //    return FromByteArray<T>(bytes);
    //}

    //public void Set(string key, object obj)
    //{
    //    var bytes = ToByteArray(obj);
    //    _cache.Set(key, bytes);
    //}

    //public async Task SetAsync(string key, object obj)
    //{
    //    var bytes = ToByteArray(obj);
    //    await _cache.SetAsync(key, bytes);
    //}


    //private T FromByteArray<T>(byte[] bytes) where T : class
    //{
    //    BinaryFormatter binaryFormatter = new BinaryFormatter();
    //    using (MemoryStream memoryStream = new MemoryStream(bytes))
    //    {
    //        return binaryFormatter.Deserialize(memoryStream) as T;
    //    }
    //}


    //private byte[] ToByteArray(object obj)
    //{
    //    if (obj == null)
    //    {
    //        return null;
    //    }
    //    BinaryFormatter binaryFormatter = new BinaryFormatter();
    //    using (MemoryStream memoryStream = new MemoryStream())
    //    {
    //        binaryFormatter.Serialize(memoryStream, obj);
    //        return memoryStream.ToArray();
    //    }
    //}

}
