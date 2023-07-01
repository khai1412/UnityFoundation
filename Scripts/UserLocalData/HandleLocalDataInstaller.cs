namespace UnityFoundation.Scripts.UserLocalData
{
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    public class HandleLocalDataInstaller : Installer<HandleLocalDataInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindAllDerivedTypes<ILocalData>();
            this.Container.Bind<HandleLocalDataServices>().AsCached().NonLazy();
            this.Container.Bind<MinimizeAppService>().FromNewComponentOnNewGameObject().AsSingle();
            typeof(ILocalData).GetDerivedTypes().ForEach(dataType=>this.Container.Resolve<HandleLocalDataServices>().Load(dataType));
        }
        
    }
}