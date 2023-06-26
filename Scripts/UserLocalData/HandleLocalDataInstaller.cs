namespace GameFoundation.Scripts.Utilities
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using ModestTree;
    using UniT.Extensions;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    public class HandleLocalDataInstaller : Installer<HandleLocalDataInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindAllDerivedTypes<ILocalData>();
            this.Container.Bind<HandleLocalDataServices>().AsCached();
        }
        
    }
}