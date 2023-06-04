namespace UnityFoundation.Scripts.UIModule.MVP.Installer
{
    using UnityFoundation.Scripts.UIModule.MVP.Manager;
    using UnityFoundation.Scripts.UIModule.MVP.Signals;
    using Zenject;

    public class ScreenFlowInstaller : Installer<ScreenFlowInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.DeclareSignal<ShowScreenSignal>();
            this.Container.DeclareSignal<CloseScreenSignal>();
            this.Container.BindInterfacesAndSelfTo<ScreenManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}