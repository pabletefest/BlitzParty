using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelHandler : MonoBehaviour
{

    //Common atributes

    [SerializeField]
    private PlayersScore scoreController;

    [SerializeField]
    private OrientationManager orientationManager;

    [SerializeField]
    private Image resultTitle;

    [SerializeField]
    private Sprite victoryImage;

    [SerializeField]
    private Sprite defeatImage;

    [SerializeField]
    private Sprite drawImage;

    //RabbitPursuit attributes

    [SerializeField]
    private ResetRabbitPursuit resetRabbitPursuitController;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    [SerializeField]
    private RectTransform joystickHandleTransform;

    //Whack-a-Mole attributes

    [SerializeField]
    private ResetWhackaMole resetWhackAMoleController;

    [SerializeField]
    private GameObject hammer;

    public void RestartButtonHandler()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuit":
                Debug.Log("1-------------------------");
                resetRabbitPursuitController.ResetGame();
                break;
            case "Whack-a-Mole":
                Debug.Log("2-------------------------");
                resetWhackAMoleController.ResetGame();
                break;
        }
        gameObject.SetActive(false);
        //ServiceLocator.Instance.GetService<ITimer>().RestartTimer();
        //SceneManager.UnloadSceneAsync("RabbitPursuit");
        //SceneManager.LoadScene("RabbitPursuit", LoadSceneMode.Additive);
        //StartCoroutine(RestartRabbitPursuitScene());
    }

    public void MenuButtonHandler()
    {
        orientationManager.ChangeScreenPortrait(true);
        SceneManager.LoadScene("MainMenu");
        //SceneManager.UnloadSceneAsync("RabbitPursuit");
    }

    public void ShowRabbitPursuitPanel()
    {
        gameObject.SetActive(true);
        //joystickHandleTransform.position = new Vector3(-8.1f, -4.7f, 0f);
        //joystick.SetActive(false);
        //joystick.GetComponent<CanvasRenderer>().SetAlpha(0);
        //joystick.GetComponent<FloatingJoystick>().enabled = false;
        joystick.GetComponent<Canvas>().enabled = false;
        catchButton.SetActive(false);
        CheckResult(scoreController.FindWinner());
    }

    public void ShowWhackAMolePanel()
    {
        gameObject.SetActive(true);
        hammer.SetActive(false);
        CheckResult(scoreController.FindWinner());
    }

    public void CheckResult(Results winner)
    {
        if (winner == Results.PLAYER1WIN)
        {
            resultTitle.sprite = victoryImage;
        }
        else if (winner == Results.PLAYER1LOSE)
        {
            resultTitle.sprite = defeatImage;
        }
        else
        {
            resultTitle.sprite = drawImage;
        }
    }
}
