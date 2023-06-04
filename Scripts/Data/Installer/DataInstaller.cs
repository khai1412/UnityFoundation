namespace Data.Installer
{
    using Data.Base;
    using Data.BlueprintData;
    using TheOneStudio.HyperCasual.Extensions;
    using UnityFoundation.Scripts.Extensions;
    using Zenject;

    public class DataInstaller : Installer<DataInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<DataManager>().AsSingle();
            typeof(IData).GetDerivedTypes().ForEach(type => this.Container.BindInterfacesAndSelfTo(type).AsSingle());
            this.Container.BindInterfacesTo<LocalResourceBlueprintJsonDataHandler>().AsSingle();
        }
    }
}