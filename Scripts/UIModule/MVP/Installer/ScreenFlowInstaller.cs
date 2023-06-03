namespace UnityFoundation.Scripts.UIModule.MVP.Installer
{
    using UnityFoundation.Scripts.UIModule.MVP.Manager;
    using Zenject;

    public class ScreenFlowInstaller : Installer<ScreenFlowInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ScreenManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}