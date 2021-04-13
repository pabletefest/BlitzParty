namespace Services
{
    public interface ISoundAdapter
    {
        void PlayMainTheme();
        void PlaySoundFX(string name);
        void Stop(string name);
        void StopMainTheme();
        bool CheckIsPlaying(string name);
        string SoundFXPlaying();
        void StopPlayingAll();
        bool CheckMainThemePlaying();
    }
}