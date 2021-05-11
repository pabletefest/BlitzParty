using UnityEngine;
using System;
using Services;

public class ResetWhackaMole : MonoBehaviour
{
    public static event Action OnSceneRestarted;

    [SerializeField]
    private PlayersScore scoreController;

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
    }
}
