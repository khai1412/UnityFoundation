namespace UnityFoundation.Scripts.GameAsset
{
    using System.Collections.Generic;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public interface IGameAssets
    {
        public List<AsyncOperationHandle<T>> PreloadAsync<T>(params object[] keys);
        AsyncOperationHandle<T>              LoadAssetAsync<T>(object key, string targetScene = "", bool isAutoUnload = true);

        public void UnloadUnusedAssets(object sceneName);
        public void ReleaseAsset(object key);
        public void UnloadSceneAsync(object key);
    }
}