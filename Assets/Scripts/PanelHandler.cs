using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelHandler : MonoBehaviour
{

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

    [SerializeField]
    private ResetRabbitPursuit resetController;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    public void RestartButtonHandler()
    {
        resetController.ResetGame();
        gameObject.SetActive(false);
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
        catchButton.SetActive(false);
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
