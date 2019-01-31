using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace UnityHelpers.Storage
{
    public class DiskCache : ICache
    {
        public string DirectoryPath { get; private set; }

        private BinaryFormatter binaryFormatter = new BinaryFormatter();

        public DiskCache(string directoryPath)
        {
            DirectoryPath = directoryPath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
#if UNITY_IOS
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
        }

        public void SetString(string key, string text)
        {
            if (text != null)
            {
                var filepath = KeyToFilePath(key);
                File.WriteAllText(filepath, text);
            }
            else
            {
                Remove(key);
            }
        }

        public void SetBytes(string key, byte[] bytes)
        {
            if (bytes != null)
            {
                var filepath = KeyToFilePath(key);
                File.WriteAllBytes(filepath, bytes);
            }
            else
            {
                Remove(key);
            }
        }

        public void SetObject(string key, object serializableObject)
        {
            if (serializableObject != null)
            {
                var filepath = KeyToFilePath(key);
                using (var file = File.Open(filepath, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(file, serializableObject);
                }
            }
            else
            {
                Remove(key);
            }
        }

        public string GetString(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath) ? File.ReadAllText(filepath) : null;
        }

        public byte[] GetBytes(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath) ? File.ReadAllBytes(filepath) : null;
        }

        public object GetObject(string key)
        {
            var filepath = KeyToFilePath(key);
            object serializedObject = null;
            if (File.Exists(filepath))
            {
                using (var file = File.Open(filepath, FileMode.Open))
                {
                    serializedObject = binaryFormatter.Deserialize(file);
                }
            }
            return serializedObject;
        }

        public bool ContainsKey(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath);
        }

        public bool Remove(string key)
        {
            var filepath = KeyToFilePath(key);
            bool exist = File.Exists(filepath);
            File.Delete(filepath);
            return exist;
        }

        public void Clear()
        {
            foreach (var filepath in Directory.EnumerateFiles(DirectoryPath))
            {
                File.Delete(filepath);
            }
        }

        public IEnumerable<string> EnumerateKeys()
        {
            return Directory.EnumerateFiles(DirectoryPath).Select(FilePathToKey);
        }

        public DateTime? GetCreateTime(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath) ? (DateTime?)File.GetCreationTimeUtc(filepath) : null;
        }

        public DateTime? GetUpdateTime(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath) ? (DateTime?)File.GetLastWriteTimeUtc(filepath) : null;
        }

        public long GetSize(string key)
        {
            var filepath = KeyToFilePath(key);
            return File.Exists(filepath) ? new FileInfo(filepath).Length : 0;
        }

        public long GetTotalSize()
        {
            long totalSize = 0;
            foreach (var filepath in Directory.EnumerateFiles(DirectoryPath))
            {
                totalSize += new FileInfo(filepath).Length;
            }
            return totalSize;
        }

        // Helpers

        private string KeyToFilePath(string key)
        {
            return Path.Combine(this.DirectoryPath, Uri.EscapeDataString(key));
        }

        private string FilePathToKey(string filepath)
        {
            return Uri.UnescapeDataString(Path.GetFileName(filepath));
        }
    }
}