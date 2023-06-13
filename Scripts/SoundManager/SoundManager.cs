namespace UnityFoundation.Scripts.SoundManager
{
    using DarkTonic.MasterAudio;

    public class SoundManager
    {
        private readonly PlaylistController       playlistController;
        private readonly MasterAudio              masterAudio;
        public SoundManager(MasterAudio masterAudio,PlaylistController playlistController)
        {
            this.masterAudio        = masterAudio;
            this.playlistController = playlistController;
        }
        
        public void PlaySound(string name,bool isLoop = false)
        {
            MasterAudio.PlaySound(name,isChaining: isLoop);
        }
        
        public virtual void PlayPlayList(string playlist, bool random = false)
        {
            this.playlistController.isShuffle = random;
            MasterAudio.StartPlaylist(playlist);
        }
        
        private void SetSoundVolume(float value)
        {
            var groups = this.masterAudio.transform.GetComponentsInChildren<MasterAudioGroup>();

            foreach (var transform in groups)
            {
                transform.groupMasterVolume = value;
            }
        }
        
        private void SetMusicVolume(float value)
        {
            this.playlistController.PlaylistVolume = value;
        }
        
        public void StopAllSound(string name) => MasterAudio.StopAllOfSound(name);
        
        public void StopPlayList(string playlist) { MasterAudio.StopPlaylist(playlist); }

        public void StopAllPlayList() { MasterAudio.StopAllPlaylists(); }
        
        public void PauseEverything()
        {
            if (this.playlistController.ControllerIsReady)
            {
                MasterAudio.MuteEverything();
            }
        }
        public void ResumeEverything()
        {
            if (this.playlistController.ControllerIsReady)
            {
                MasterAudio.UnmuteEverything();
            }
        }
    }
}