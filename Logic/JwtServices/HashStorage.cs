using System.Collections.Generic;

namespace Logic.JwtServices
{
    public class HashStorage
    {
        private readonly Dictionary<string, string> storage = new();

        public string GetHash(string key)
        {
            storage.TryGetValue(key, out string value);

            return value;
        }

        public void SetHash(string key, string value)
        {
            if (!storage.TryAdd(key, value))
                storage[key] = value;
        }
    }
}
