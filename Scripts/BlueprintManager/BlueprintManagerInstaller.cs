namespace UnityFoundation.Scripts.BlueprintManager
{
    using GameFoundation.Scripts.Utilities.Extension;
    using UnityFoundation.Scripts.BlueprintManager.BlueprintBase;
    using UnityFoundation.Scripts.BlueprintManager.HandleBlueprintData;
    using Zenject;

    public class BlueprintManagerInstaller: Installer<BlueprintManagerInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindAllDerivedTypes<IBlueprintData>();
            this.Container.BindAllDerivedTypes<IHandleBlueprintData>();
            this.Container.Bind<BlueprintDataManager>().AsCached();
            this.Container.DeclareSignal<LoadedAllBlueprintDataSignal>();
        }
    }
}