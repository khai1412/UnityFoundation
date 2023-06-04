namespace UnityFoundation.Scripts.Installer
{
    using GameFoundation.Scripts.UIModule;
    using UnityEngine;
    using UnityFoundation.Scripts.UIModule.MVP.Manager;
    using Zenject;

    public abstract class BaseSceneInstaller : MonoInstaller
    {
        [SerializeField] protected RootUICanvas rootUICanvas;

        [Inject] private ScreenManager screenManager;
        public override void InstallBindings()
        {
            if(this.rootUICanvas == null) return;
            this.screenManager.RootUICanvas       = this.rootUICanvas;
            this.screenManager.CurrentRootScreen  = this.rootUICanvas.RootUIShowTransform;
            this.screenManager.CurrentHiddenRoot  = this.rootUICanvas.RootUIClosedTransform;
            this.screenManager.CurrentOverlayRoot = this.rootUICanvas.RootUIOverlayTransform;
        }
    }
}