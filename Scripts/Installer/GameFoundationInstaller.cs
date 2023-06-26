using UnityFoundation.Scripts.GameAsset;

namespace GameFoundation.Scripts.Installer
{
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Scenes.Test;
    using UnityFoundation.Scripts.BlueprintManager;
    using UnityFoundation.Scripts.UIModule.MVP.Installer;
    using Zenject;

    public class GameFoundationInstaller : Installer<GameFoundationInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(this.Container);
            ScreenFlowInstaller.Install(this.Container);
            BlueprintManagerInstaller.Install(this.Container);
            HandleLocalDataInstaller.Install(this.Container);
            this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached();
            this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy();
            this.Container.Bind<Fps>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            this.Container.Bind<HandleLocalDataServices>().AsCached().NonLazy();
            this.Container.Bind<AudioManager>().AsCached();
        }
    }
}