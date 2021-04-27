using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Services;

public class ResetWhackaMole : MonoBehaviour
{
    public static event Action OnSceneRestarted;

    [SerializeField]
    private PlayersScore scoreController;

    [SerializeField]
    private GameObject hammer;

    public void ResetGame()
    {
        Time.timeScale = 1f;
        scoreController.ResetWhackAMoleScore();
        OnSceneRestarted?.Invoke();
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
        hammer.SetActive(true);
    }
}
