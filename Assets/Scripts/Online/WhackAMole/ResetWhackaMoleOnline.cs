using System;
using Services;
using UnityEngine;
using WhackAMole;

namespace Online.WhackAMole
{
    public class ResetWhackaMoleOnline : MonoBehaviour
    {
        public static event Action OnSceneRestarted;

        [SerializeField]
        private PlayersScore scoreController;

        [SerializeField]
        private GameObject pauseButton;

        [SerializeField]
        private HammerSpawnerOnline hammerSpawner;

        private void Awake()
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("Whack-a-moleTheme");
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

