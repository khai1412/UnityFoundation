﻿namespace GameFoundation.Scripts.Installer
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityFoundation.Scripts.UIModule.MVP.Installer;
    using Zenject;

    public class GameFoundationInstaller : Installer<GameFoundationInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(this.Container);
            ScreenFlowInstaller.Install(this.Container);
            this.Container.Bind<IGameAssets>().To<GameAssets>().AsCached();
            this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy();
            this.Container.Bind<Fps>().FromNewComponentOnNewGameObject().AsCached().NonLazy();
            this.Container.Bind<HandleLocalDataServices>().AsCached().NonLazy();
        }
    }
}