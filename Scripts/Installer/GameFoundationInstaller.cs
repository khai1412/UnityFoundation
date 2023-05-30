namespace GameFoundation.Scripts.Installer
{
    using GameFoundation.Scripts.ObjectPoolManager;
    using Zenject;

    public class GameFoundationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.Bind<ObjectPoolManager>().AsCached().NonLazy();
        }
    }
}