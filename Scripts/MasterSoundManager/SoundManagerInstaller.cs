namespace UnityFoundation.Scripts.SoundManager
{
    using DarkTonic.MasterAudio;
    using Zenject;

    public class SoundManagerInstaller : Installer<SoundManagerInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<PlaylistController>().FromComponentInNewPrefabResource("GamePlaylistController").AsCached().NonLazy();
            this.Container.Bind<MasterAudio>().FromComponentInNewPrefabResource("GameMasterAudio").AsCached().NonLazy();
        }
    }
}