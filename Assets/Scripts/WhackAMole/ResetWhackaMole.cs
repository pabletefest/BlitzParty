using UnityEngine;
using System;
using Services;

namespace WhackAMole
{
    public class ResetWhackaMole : MonoBehaviour
    {
        public static event Action OnSceneRestarted;

        [SerializeField]
        private PlayersScore scoreController;

        [SerializeField]
        private GameObject pauseButton;

        [SerializeField]
        private HammerSpawner hammerSpawner;

        private void Awake()
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("Whack-a-moleTheme");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    
        public void ResetGame()
        {
            scoreController.ResetWhackAMoleScore();
            OnSceneRestarted?.Invoke();
            ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
            Time.timeScale = 1f;
            pauseButton.SetActive(true);
            hammerSpawner.enabled = true;
        }
    }
}

