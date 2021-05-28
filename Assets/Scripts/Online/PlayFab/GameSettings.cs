namespace Online.PlayFab
{
    public class GameSettings
    {
        private float musicVolume;
        private float sfxVolume;

        public float MusicVolume => musicVolume;
        public float SFXVolume => sfxVolume;
        
        public GameSettings(float musicVolume, float sfxVolume)
        {
            this.musicVolume = musicVolume;
            this.sfxVolume = sfxVolume;
        }
    }
}