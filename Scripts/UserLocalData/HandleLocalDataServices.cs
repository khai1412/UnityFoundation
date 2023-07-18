namespace UnityFoundation.Scripts.UserLocalData
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using Newtonsoft.Json;
    using UnityEngine;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    /// <summary>
    /// Manager save Load Local data
    /// </summary>
    public class HandleLocalDataServices
    {
        private Dictionary<string, ILocalData> localDataCaches = new();
        public void AddOrUpdateLocalData(string key, ILocalData localData)
        {
            if (this.localDataCaches.TryGetValue(key, out var data))
            {
                this.localDataCaches[key] = data;
                return;
            }

            this.localDataCaches.Add(key, localData);
        }


        public void SaveLocalData<T>(T localData) where T : ILocalData
        {
            var key = typeof(T).Name;
            this.AddOrUpdateLocalData(key, localData);
            var json = JsonConvert.SerializeObject(localData);
            PlayerPrefs.SetString(key, json);
            Debug.Log("Save " + key + ": " + json);
            PlayerPrefs.Save();
        }

        public ILocalData Load(Type type)
        {
            var key = type.Name;

            if (this.localDataCaches.TryGetValue(key, out var cache))
            {
                return cache;
            }

            var json = PlayerPrefs.GetString(key);

            var result = string.IsNullOrEmpty(json) ? this.GetCurrentContainer().Resolve(type) : JsonConvert.DeserializeObject(json, type);

            this.AddOrUpdateLocalData(key, (ILocalData)result);
            return (ILocalData)result;
        }


        public void StoreAllToLocal()
        {
            foreach (var localData in this.localDataCaches)
            {
                PlayerPrefs.SetString(localData.Key, JsonConvert.SerializeObject(localData.Value));
            }

            PlayerPrefs.Save();
            Debug.Log("Save Data To File");
        }
    }
}