namespace UnityFoundation.Scripts.UserLocalData
{
    using System;
    using GameFoundation.Scripts.Utilities;
    using UnityEngine;
    using Zenject;

    /// <summary>Catch application event ex pause, focus and more.... </summary>
    public class MinimizeAppService : MonoBehaviour
    {
        [Inject] private SignalBus               signalBus;
        [Inject] private HandleLocalDataServices localDataServices;
        
        private void OnApplicationPause(bool pauseStatus)
        {
            this.localDataServices.StoreAllToLocal();
        }

        private void OnApplicationQuit()
        {
            Debug.LogError("dmm quit");
            this.localDataServices.StoreAllToLocal();
        }
    }
}