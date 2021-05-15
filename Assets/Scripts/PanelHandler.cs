using RabbitPursuit;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WhackAMole;

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

    [SerializeField]
    private EarnAcorns earnAcorns;

    [SerializeField]
    private Text acornsText;

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

    public void RestartButtonHandler()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuit":
                resetRabbitPursuitController.ResetGame();
                break;
            case "Whack-a-Mole":
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
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        orientationManager.ChangeScreenPortrait(true);
        SceneManager.LoadScene("MainMenu");
        ServiceLocator.Instance.GetService<IObjectPooler>().ClearAllPools();
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme();
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
        acornsText.text = earnAcorns.CalculateAcornsEarned("RabbitPursuit").ToString();
    }

    public void ShowWhackAMolePanel()
    {
        gameObject.SetActive(true);
        DestroyRemainingHammers();
        CheckResult(scoreController.FindWinner());
        acornsText.text = earnAcorns.CalculateAcornsEarned("WhackAMole").ToString();
    }

    public void CheckResult(Results winner)
    {
        if (winner == Results.PLAYER1WIN)
        {
            resultTitle.sprite = victoryImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
        }
        else if (winner == Results.PLAYER1LOSE)
        {
            resultTitle.sprite = defeatImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Lose");
        }
        else
        {
            resultTitle.sprite = drawImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Draw");
        }
    }
    
    private void DestroyRemainingHammers()
    {
        GameObject[] hammers = GameObject.FindGameObjectsWithTag("Hammer");

        foreach (var hammer in hammers)
        {
            Destroy(hammer);
        }
    }
}
