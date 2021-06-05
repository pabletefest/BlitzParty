using Mirror;
using Online;
using Online.BinkyPursuit;
using Online.WhackAMole;
using RabbitPursuit;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WhackAMole;

public class PanelHandlerOnline : NetworkBehaviour
{
    //Common atributes

    [SerializeField]
    private PlayersScoreOnline scoreController;

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
    private EarnAcornsOnline earnAcorns;

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
    private ResetRabbitPursuitOnline resetRabbitPursuitController;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    [SerializeField]
    private GameFinisher cowboyDuelFinisher;

    //Whack-a-Mole attributes

    [SerializeField]
    private ResetWhackaMoleOnline resetWhackAMoleController;

    [SerializeField]
    private HammerSpawnerOnline hammerSpawner;

    [SerializeField] private GameObject panel;
    
    public override void OnStartClient()
    {
        //gameObject.SetActive(false);
    }

    public override void OnStartAuthority()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuitOnline":
                ShowRabbitPursuitPanel();
                break;
            case "WhackAMoleOnline":
                ShowWhackAMolePanel();
                break;
            case "CowboyDuelOnline":
                ShowCowboyDuelPanel();
                break;
        }
    }

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

    public void ShowRabbitPursuitPanel()
    {
        panel.SetActive(true);
        //gameObject.SetActive(true);
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
    }

    public void ShowWhackAMolePanel()
    {
        Debug.Log($"Is server calling this? {isServer}");
        if (!isServer) return;
        
        panel.SetActive(true);
        //gameObject.SetActive(true);
        pauseButton.SetActive(false);
        /*
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
        */
        
        CheckClientsResult();
        //CheckResult(scoreController.FindWinner(), "WhackAMole");
        //acornsText.text = earnAcorns.CalculateAcornsEarned("WhackAMole").ToString();
        //earnAcorns.AcornsWhackAMole();
        //database.AddPlayerWhackAMoleGames();
    }

    //[Command(requiresAuthority = false)]
    private void CheckClientsResult()
    {
        Debug.Log("Checking results...");
        CheckResult(scoreController.FindWinner(), "WhackAMole");
    }

    public void ShowCowboyDuelPanel()
    {
        panel.SetActive(true);
        //gameObject.SetActive(true);
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
    }

    public void CheckResult(Results winner, string minigame)
    {
        if (!isServer) return;
        
        if (winner == Results.PLAYER1WIN)
        {
            if (database.IsBattleMode())
            {
                database.UpdatePlayer1BattleWins(database.LoadPlayer1BattleWins() + 1);
            }
            
            //resultTitle.sprite = victoryImage;
            //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
            //UpdateWins(minigame);

            NetworkConnection player1Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).playersConnections[1];
            SetClientResult(player1Conn, minigame, true);
            
            NetworkConnection player2Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).playersConnections[2];
            SetClientResult(player2Conn, minigame, false);
        }
        else if (winner == Results.PLAYER2WIN)
        {
            if (database.IsBattleMode())
            {
                database.UpdatePlayer2BattleWins(database.LoadPlayer2BattleWins() + 1);
            }
            
            //resultTitle.sprite = defeatImage;
            //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Lose");
            NetworkConnection player1Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).playersConnections[1];
            SetClientResult(player1Conn, minigame, false);
            
            
            NetworkConnection player2Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).playersConnections[2];
            SetClientResult(player2Conn, minigame, true);
        }
        else
        {
            //resultTitle.sprite = drawImage;
            //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Draw");
            ClientsDrawGame();
        }
    }

    [TargetRpc]
    private void SetClientResult(NetworkConnection target, string minigame, bool isWinner)
    {
        if (isWinner)
        {
            resultTitle.sprite = victoryImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
        }
        else
        {
            resultTitle.sprite = defeatImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Lose");
        }
        
        panel.SetActive(true);
        pauseButton.SetActive(false);
        
        UpdateWins(minigame);
    }

    [ClientRpc]
    private void ClientsDrawGame()
    {
        resultTitle.sprite = drawImage;
        
        panel.SetActive(true);
        pauseButton.SetActive(false);
        
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Draw");
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