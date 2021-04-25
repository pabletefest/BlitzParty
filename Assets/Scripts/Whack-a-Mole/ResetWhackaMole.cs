using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Services;

public class ResetWhackaMole : MonoBehaviour
{

    public static event Action<string> OnSceneRestarted;

    [SerializeField]
    private PlayersScore scoreController;

    private void Start()
    {

    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        scoreController.ResetScore();
        OnSceneRestarted?.Invoke(SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
    }
}
