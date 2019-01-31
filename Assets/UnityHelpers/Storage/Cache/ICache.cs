using System;
using System.Collections.Generic;

namespace UnityHelpers.Storage
{
    public interface ICache
    {
        void SetString(string key, string text);
        void SetBytes(string key, byte[] bytes);
        void SetObject(string key, object serializableObject);

        string GetString(string key);
        byte[] GetBytes(string key);
        object GetObject(string key);

        bool ContainsKey(string key);
        bool Remove(string key);
        void Clear();

        IEnumerable<string> EnumerateKeys();

        DateTime? GetCreateTime(string key);
        DateTime? GetUpdateTime(string key);
        long GetSize(string key);

        long GetTotalSize();
    }
}