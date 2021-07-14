using Online.PlayFab;
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

    [SerializeField]
    private HammerSpawner hammerSpawner;

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
        ServiceLocator.Instance.GetService<IObjectPooler>().ClearAllPools();
        battleModeHandler.StartNextMinigame();
    }
    
    private void StoreUserDataOnCloud()
    {
        string playFabId = database.GetPlayFabId();
        string username = database.GetUsername();
        
        int acorns = database.LoadAcorns();
        
        float musicVolume = database.LoadMusicVolume();
        float sfxVolume = database.LoadSFXVolume();
        GameSettings gameSettings = new GameSettings(musicVolume, sfxVolume);

        int RabbitGames = database.LoadPlayerRabbitPursuitGames();
        int MoleGames = database.LoadPlayerWhackAMoleGames();
        int CowboyGames = database.LoadPlayerCowboyDuelGames();
        int RabbitWins = database.LoadPlayerRabbitPursuitWins();
        int MoleWins = database.LoadPlayerWhackAMoleWins();
        int CowboyWins = database.LoadPlayerCowboyDuelWins();

        CloudStoragePlayFab cloudStorage = new CloudStoragePlayFab();

        cloudStorage.SetUserData(playFabId, username, acorns, gameSettings, RabbitGames, MoleGames, CowboyGames, RabbitWins, MoleWins, CowboyWins);
    }

    public void ShowRabbitPursuitPanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            if (database.LoadCurrentBattleStage() == 3)
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Show Results Button");
            }
            else
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Next Minigame Button");
            }
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
        StoreUserDataOnCloud();
    }

    public void ShowWhackAMolePanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            if (database.LoadCurrentBattleStage() == 3)
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Show Results Button");
            }
            else
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Next Minigame Button");
            }
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
        StoreUserDataOnCloud();
    }

    public void ShowCowboyDuelPanel()
    {
        gameObject.SetActive(true);
        pauseButton.SetActive(false);
        if (database.IsBattleMode())
        {
            menuButton.SetActive(false);
            restartButton.SetActive(false);
            if (database.LoadCurrentBattleStage() == 3)
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Show Results Button");
            }
            else
            {
                nextMinigameButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Next Minigame Button");
            }
            nextMinigameButton.SetActive(true);
        }
        else
        {
            menuButton.SetActive(true);
            restartButton.SetActive(true);
            nextMinigameButton.SetActive(false);
        }
        CheckResult(scoreController.FindWinner(), "CowboyDuel");
        acornsText.text = earnAcorns.CalculateAcornsEarned("CowboyDuel").ToString();
        earnAcorns.AcornsCowboyDuel();
        database.AddPlayerCowboyDuelGames();
        StoreUserDataOnCloud();
    }

    public void CheckResult(Results winner, string minigame)
    {
        if (winner == Results.PLAYER1WIN)
        {
            if (database.IsBattleMode())
            {
                database.UpdatePlayer1BattleWins(database.LoadPlayer1BattleWins() + 1);
            }
            resultTitle.sprite = victoryImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
            UpdateWins(minigame);
        }
        else if (winner == Results.PLAYER1LOSE)
        {
            if (database.IsBattleMode())
            {
                database.UpdatePlayer2BattleWins(database.LoadPlayer2BattleWins() + 1);
            }
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

        hammerSpawner.enabled = false;
    }
}
