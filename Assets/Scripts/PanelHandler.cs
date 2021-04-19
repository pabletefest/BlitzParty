using System;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelHandler : MonoBehaviour
{
    public static event Action<string> OnSceneRestarted;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private PlayersScore scoreController;

    [SerializeField]
    private Image resultTitle;

    [SerializeField]
    private Sprite victoryImage;

    [SerializeField]
    private Sprite defeatImage;

    [SerializeField]
    private Sprite drawImage;

    public void RestartButtonHandler()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        scoreController.ResetScore();
        OnSceneRestarted?.Invoke(SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
        joystick.SetActive(true);
        //ServiceLocator.Instance.GetService<ITimer>().RestartTimer();
        //SceneManager.UnloadSceneAsync("RabbitPursuit");
        //SceneManager.LoadScene("RabbitPursuit", LoadSceneMode.Additive);
        //StartCoroutine(RestartRabbitPursuitScene());
    }

    public void MenuButtonHandler()
    {
        SceneManager.LoadScene("MainMenu");
        //SceneManager.UnloadSceneAsync("RabbitPursuit");
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        joystick.SetActive(false);
        GameObject.Find("CatchButton").SetActive(false);
        CheckResult();

    }

    private void CheckResult()
    {
        RabbitPursuitResults winner = scoreController.FindWinner();

        if(winner == RabbitPursuitResults.PLAYER1WIN)
        {
            resultTitle.sprite = victoryImage;
        }
        else if(winner == RabbitPursuitResults.PLAYER2WIN)
        {
            resultTitle.sprite = defeatImage;
        }
        else
        {
            resultTitle.sprite = drawImage;
        }

    }
}
