using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace UnityFoundation.Scripts.GameAsset
{
    using System;

    public class GameAssets : IGameAssets
    {
        private Dictionary<object, AsyncOperationHandle> loadedAssets       = new();
        private Dictionary<object, AsyncOperationHandle> loadingAssets      = new();
        private Dictionary<object, List<object>>         assetLoadedByScene = new();
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activeOnLoad = true)
        {
            if (this.loadedAssets.TryGetValue(key, out var loadAssetHandle)) return loadAssetHandle.Convert<SceneInstance>();
            if (this.loadingAssets.TryGetValue(key, out var loadingAssetHandle)) return loadingAssetHandle.Convert<SceneInstance>();
            var handle = Addressables.LoadSceneAsync(key, loadMode, activeOnLoad);
            this.loadingAssets.Add(key, handle);
            handle.Completed += (asyncOperationHandle) =>
            {
                this.loadedAssets.Add(key, asyncOperationHandle);
                this.loadingAssets.Remove(key);
                this.assetLoadedByScene.Add(key, new List<object>());
            };
            return handle;
        }

        public void UnloadSceneAsync(object key)
        {
            AsyncOperationHandle<SceneInstance> sceneHandle = default;
            if (!this.assetLoadedByScene.TryGetValue(key.ToString(), out var listHandle)) return;
            if (this.loadedAssets.TryGetValue(key, out var loadedHandle)) sceneHandle   = loadedHandle.Convert<SceneInstance>();
            if (this.loadingAssets.TryGetValue(key, out var loadingHandle)) sceneHandle = loadingHandle.Convert<SceneInstance>();
            Addressables.UnloadSceneAsync(sceneHandle);
            this.UnloadUnusedAssets(key);
            this.assetLoadedByScene.Remove(key);
        }

        public void UnloadUnusedAssets(object sceneName)
        {
            if (!this.assetLoadedByScene.ContainsKey(sceneName)) return;
            this.assetLoadedByScene[sceneName].ForEach(e =>
            {
                this.ReleaseAsset(e);
                this.TryRemoveHandle(e);
            });
        }

        public List<AsyncOperationHandle<T>> PreloadAsync<T>(params object[] keys) { return keys.Select(key => Addressables.LoadAssetAsync<T>(key)).ToList(); }

        public AsyncOperationHandle<T> LoadAssetAsync<T>(object key, string targetScene, bool isAutoUnload = true)
        {
            if (this.loadedAssets.TryGetValue(key, out var loadAssetHandle)) return loadAssetHandle.Convert<T>();
            if (this.loadingAssets.TryGetValue(key, out var loadingAssetHandle)) return loadingAssetHandle.Convert<T>();
            var handle = Addressables.LoadAssetAsync<T>(key);
            this.loadingAssets.Add(key, handle);
            handle.Completed += (asyncOperationHandle) =>
            {
                this.loadedAssets.Add(key, asyncOperationHandle);
                this.loadingAssets.Remove(key);
                this.AddAssetToScene(targetScene, key);
            };
            return handle;
        }

        private void AddAssetToScene(string targetScene, object key)
        {
            if (this.assetLoadedByScene.TryGetValue(targetScene, out var assetKey))
            {
                this.assetLoadedByScene[targetScene].Add(assetKey);
                return;
            }

            this.assetLoadedByScene.Add(targetScene, new List<object>());
        }

        public void ReleaseAsset(object key)
        {
            if (!this.loadedAssets.TryGetValue(key, out var handle)) return;
            try
            {
                if(!handle.IsValid()) return;
                Addressables.Release(handle);
                this.loadedAssets.Remove(key);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void TryRemoveHandle(object key)
        {
            if (this.loadedAssets.ContainsKey(key)) this.loadedAssets.Remove(key);
            if (this.loadingAssets.ContainsKey(key)) this.loadingAssets.Remove(key);
            foreach (var scene in this.assetLoadedByScene.Where(scene => scene.Value.Contains(key))) scene.Value.Remove(key);
        }
    }
}