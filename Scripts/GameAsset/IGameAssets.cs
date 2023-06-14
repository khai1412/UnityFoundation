namespace UnityFoundation.Scripts.GameAsset
{
    using System.Collections.Generic;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public interface IGameAssets
    {
        public List<AsyncOperationHandle<T>> PreloadAsync<T>(params object[] keys);
        AsyncOperationHandle<T>              LoadAssetAsync<T>(object key, string targetScene = "", bool isAutoUnload = true);

        public void UnloadUnusedAssets(object sceneName);
        public void ReleaseAsset(object key);
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activeOnLoad = true);
        public void UnloadSceneAsync(object key);
    }
}