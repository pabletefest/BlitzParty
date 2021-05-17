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

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject pauseButton;

    [SerializeField]
    private GameObject menuButton;

    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private GameObject nextMinigameButton;

    [SerializeField]
    private BattleModeHandler battleModeHandler;

    //RabbitPursuit attributes

    [SerializeField]
    private ResetRabbitPursuit resetRabbitPursuitController;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    [SerializeField]
    private GameFinisher cowboyDuelFinisher;

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
            case "CowboyDuel":
                cowboyDuelFinisher.GameRestarter();
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

    public void NextMinigameButtonHandler()
    {
        battleModeHandler.StartNextMinigame();
    }

    public void ShowRabbitPursuitPanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            nextMinigameButton.SetActive(true);
        }
        else
        {
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            nextMinigameButton.SetActive(false);
        }

        joystick.GetComponent<Canvas>().enabled = false;
        catchButton.SetActive(false);
        CheckResult(scoreController.FindWinner(), "RabbitPursuit");
        acornsText.text = earnAcorns.CalculateAcornsEarned("RabbitPursuit").ToString();
        earnAcorns.AcornsRabbitPursuit();
        database.AddPlayerRabbitPursuitGames();
    }

    public void ShowWhackAMolePanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            nextMinigameButton.SetActive(true);
        }
        else
        {
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            nextMinigameButton.SetActive(false);
        }
        DestroyRemainingHammers();
        CheckResult(scoreController.FindWinner(), "WhackAMole");
        acornsText.text = earnAcorns.CalculateAcornsEarned("WhackAMole").ToString();
        earnAcorns.AcornsWhackAMole();
        database.AddPlayerWhackAMoleGames();
    }

    public void ShowCowboyDuelPanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            //nextMinigameButton.SetActive(true);
        }
        else
        {
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            //nextMinigameButton.SetActive(false);
        }
        CheckResult(scoreController.FindWinner(), "CowboyDuel");
        acornsText.text = earnAcorns.CalculateAcornsEarned("CowboyDuel").ToString();
        earnAcorns.AcornsCowboyDuel();
        database.AddPlayerCowboyDuelGames();
    }

    public void CheckResult(Results winner, string minigame)
    {
        if (winner == Results.PLAYER1WIN)
        {
            resultTitle.sprite = victoryImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
            UpdateWins(minigame);
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

    private void UpdateWins(string minigame)
    {
        switch (minigame)
        {
            case "RabbitPursuit":
                database.AddPlayerRabbitPursuitWins();
                break;
            case "WhackAMole":
                database.AddPlayerWhackAMoleWins();
                break;
            case "CowboyDuel":
                database.AddPlayerCowboyDuelWins();
                break;
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
