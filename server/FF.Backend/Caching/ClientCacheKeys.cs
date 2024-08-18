using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Backend.CacheKeys
{
    public static class ClientCacheKeys
    {
        public static string GetClientKey(string code) => $"GetClient-{code}";
    }
}
