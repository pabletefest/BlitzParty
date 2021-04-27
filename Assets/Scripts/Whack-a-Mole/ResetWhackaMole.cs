using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Services;

public class ResetWhackaMole : MonoBehaviour
{
    public static event Action OnSceneRestarted;

    [SerializeField]
    private PlayersScore scoreController;

    public void ResetGame()
    {
        scoreController.ResetWhackAMoleScore();
        OnSceneRestarted?.Invoke();
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
        Time.timeScale = 1f;
    }
}
