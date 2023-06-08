using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace UnityFoundation.Scripts.GameAsset
{
    public class GameAssets : IGameAssets
    {
        public Dictionary<object, AsyncOperationHandle> LoadedAssets;
        public Dictionary<object, AsyncOperationHandle> LoadingAsset;
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activeOnLoad = true)
        {
            throw new System.NotImplementedException();
        }

        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(object key)
        {
            throw new System.NotImplementedException();
        }

        public void UnloadUnusedAssets(string sceneName)
        {
            throw new System.NotImplementedException();
        }

        public List<AsyncOperationHandle<T>> PreloadAsync<T>(string targetScene = "", params object[] keys)
        {
            return keys.Select(key => Addressables.LoadAssetAsync<T>(key)).ToList();
        }

        public AsyncOperationHandle<T> LoadAssetAsync<T>(object key, bool isAutoUnload = true, string targetScene = "")
        {
            if (this.LoadedAssets.TryGetValue(key, out var loadAssetHandle)) return loadAssetHandle.Convert<T>();
            var handle = Addressables.LoadAssetAsync<T>(key);
            this.LoadingAsset.Add(key,handle);
            handle.Completed += (asyncOperationHandle) =>
            {
                this.LoadedAssets.Add(key,asyncOperationHandle);
                this.LoadingAsset.Remove(key);
            };
            return handle;
        }

        public void ReleaseAsset(object key)
        {
            if (LoadedAssets.TryGetValue(key, out var handle))
            {
                Addressables.Release(key);
                this.LoadedAssets.Remove(handle);
            }
        }
        
    }
}