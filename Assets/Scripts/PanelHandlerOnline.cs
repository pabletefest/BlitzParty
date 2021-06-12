using System;
using Mirror;
using Online;
using Online.BinkyPursuit;
using Online.CowboyDuel;
using Online.WhackAMole;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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

    [FormerlySerializedAs("pauseButton")] [SerializeField]
    private GameObject settingsButton;

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
    private GameObject catchButton2;

    [SerializeField]
    private GameFinisher cowboyDuelFinisher;

    //Whack-a-Mole attributes

    [SerializeField]
    private ResetWhackaMoleOnline resetWhackAMoleController;

    [SerializeField]
    private HammerSpawnerOnline hammerSpawner;

    [SerializeField] private GameObject panel;
    
    [SerializeField] private GameObject multiplayerMessagePanel;
    
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

    [ClientRpc]
    public void RpcActivateBinkyPursuitVisualElements()
    {
        Debug.Log("Activating joystick and pause button");
        joystick.GetComponent<Canvas>().enabled = true;
        //catchButton.GetComponent<Image>().enabled = true;
        //catchButton.GetComponent<Button>().enabled = true;
        settingsButton.SetActive(true);
        //AnchorCatchButtonToPlayer();
    }

    [TargetRpc]
    public void RpcAnchorCatchButtonToPlayer(NetworkConnection target)
    {
        Debug.Log("Activating catch buttons for players");
        Debug.Log($"I'm connection {target}");

        PlayerMovementOnline player = target.identity.GetComponent<PlayerMovementOnline>();

        int playerNumber = player.playerNumber;

        if (playerNumber == 1)
        {
            catchButton.GetComponent<Image>().enabled = true;
            Button button = catchButton.GetComponent<Button>();
            button.enabled = true;
            button.onClick.AddListener(player.CatchButtonHandler);
        }
        else if (playerNumber == 2)
        {
            catchButton2.GetComponent<Image>().enabled = true;
            Button button = catchButton2.GetComponent<Button>();
            button.enabled = true;
            button.onClick.AddListener(player.CatchButtonHandler);
        }
    }
    
    [ClientRpc]
    public void RpcActivateWhackAMoleVisualElements()
    {
        settingsButton.SetActive(true);
    }
    
    [ClientRpc]
    public void RpcActivateCowboyDuelVisualElements()
    {
        settingsButton.SetActive(true);
    }

    [ClientRpc]
    public void RpcDisableWaitingPlayersPanel()
    {
        multiplayerMessagePanel.SetActive(false);
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
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme();
    }

    public void NextMinigameButtonHandler()
    {
        ServiceLocator.Instance.GetService<IObjectPooler>().ClearAllPools();
        battleModeHandler.StartNextMinigame();
    }

    public void ShowRabbitPursuitPanel()
    {
        if (!isServer) return;
        
        panel.SetActive(true);
        //gameObject.SetActive(true);
        settingsButton.SetActive(false);
        /*if (database.IsBattleMode())
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
        }*/

        Destroy(joystick);
        Destroy(catchButton);
        //joystick.GetComponent<Canvas>().enabled = false;
        //catchButton.SetActive(false);
        CheckClientsResult("RabbitPursuit");
        // acornsText.text = earnAcorns.CalculateAcornsEarned("RabbitPursuit").ToString();
        // earnAcorns.AcornsRabbitPursuit();
        // database.AddPlayerRabbitPursuitGames();
    }

    public void ShowWhackAMolePanel()
    {
        Debug.Log($"Is server calling this? {isServer}");
        if (!isServer) return;
        
        panel.SetActive(true);
        //gameObject.SetActive(true);
        settingsButton.SetActive(false);
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
        
        CheckClientsResult("WhackAMole");
        //CheckResult(scoreController.FindWinner(), "WhackAMole");
        //acornsText.text = earnAcorns.CalculateAcornsEarned("WhackAMole").ToString();
        //earnAcorns.AcornsWhackAMole();
        //database.AddPlayerWhackAMoleGames();
    }


    public void ShowCowboyDuelPanel()
    {
        if (!isServer) return;
        
        panel.SetActive(true);
        //gameObject.SetActive(true);
        settingsButton.SetActive(false);
        /*if (database.IsBattleMode())
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
        }*/
        CheckResult(scoreController.FindWinner(), "CowboyDuel");
        // acornsText.text = earnAcorns.CalculateAcornsEarned("CowboyDuel").ToString();
        // earnAcorns.AcornsCowboyDuel();
        // database.AddPlayerCowboyDuelGames();
    }

    //[Command(requiresAuthority = false)]
    private void CheckClientsResult(string minigame)
    {
        Debug.Log("Checking results...");
        CheckResult(scoreController.FindWinner(), minigame);
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
            SetClientResultByMinigame(minigame, true, false);
        }
        else if (winner == Results.PLAYER2WIN)
        {
            if (database.IsBattleMode())
            {
                database.UpdatePlayer2BattleWins(database.LoadPlayer2BattleWins() + 1);
            }
            
            //resultTitle.sprite = defeatImage;
            //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Lose");
            SetClientResultByMinigame(minigame, false, true);
        }
        else
        {
            //resultTitle.sprite = drawImage;
            //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Draw");
            ClientsDrawGame(minigame);
        }
    }

    private void SetClientResultByMinigame(string minigame, bool isP1Winner, bool isP2Winner)
    {
        NetworkConnection player1Conn = null;

        NetworkConnection player2Conn = null;
            
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuitOnline":
                player1Conn = ((RabbitPursuitNetworkManager) NetworkManager.singleton).PlayersConnections[1];
                SetClientResult(player1Conn, minigame, isP1Winner);
            
                player2Conn = ((RabbitPursuitNetworkManager) NetworkManager.singleton).PlayersConnections[2];
                SetClientResult(player2Conn, minigame, isP2Winner);
                break;
            case "WhackAMoleOnline":
                player1Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).PlayersConnections[1];
                SetClientResult(player1Conn, minigame, isP1Winner);
            
                player2Conn = ((WhackAMoleNetworkManager) NetworkManager.singleton).PlayersConnections[2];
                SetClientResult(player2Conn, minigame, isP2Winner);
                break;
            case "CowboyDuelOnline":
                player1Conn = ((CowboyDuelNetworkManager) NetworkManager.singleton).PlayersConnections[1];
                SetClientResult(player1Conn, minigame, isP1Winner);
            
                player2Conn = ((CowboyDuelNetworkManager) NetworkManager.singleton).PlayersConnections[2];
                SetClientResult(player2Conn, minigame, isP2Winner);
                break;
        }
    }
    
    [TargetRpc]
    private void SetClientResult(NetworkConnection target, string minigame, bool isWinner)
    {
        Debug.Log($"Am in server: {isServer}");
        Debug.Log("Someone won");
        if (isWinner)
        {
            resultTitle.sprite = victoryImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Win");
            
            ObtainAcornsEarnedCommand(minigame);
        }
        else
        {
            resultTitle.sprite = defeatImage;
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Lose");
        }
        
        panel.SetActive(true);
        settingsButton.SetActive(false);
        
        UpdateWins(minigame);
    }

    [ClientRpc]
    private void ClientsDrawGame(string minigame)
    {
        Debug.Log($"Am in server: {isServer}");
        Debug.Log("Game draw");
        resultTitle.sprite = drawImage;
        
        int acornsEarned = 10;
        acornsText.text = acornsEarned.ToString();
        
        switch (minigame)
        {
            case "RabbitPursuit":
                earnAcorns.AcornsRabbitPursuit(acornsEarned);
                database.AddPlayerRabbitPursuitGames();
                break;
            case "WhackAMole":
                earnAcorns.AcornsWhackAMole(acornsEarned);
                database.AddPlayerWhackAMoleGames();
                break;
            case "CowboyDuel":
                earnAcorns.AcornsCowboyDuel(acornsEarned);
                database.AddPlayerCowboyDuelGames();
                break;
        }

        panel.SetActive(true);
        settingsButton.SetActive(false);
        
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("Draw");
    }

    [Command(requiresAuthority = false)]
    private void ObtainAcornsEarnedCommand(string minigame, NetworkConnectionToClient sender = null)
    {
        int playerNumber = 0;
        int acornsEarned = 0;
        
        switch (minigame)
        {
            case "RabbitPursuit":
                playerNumber = sender.identity.gameObject.GetComponent<PlayerMovementOnline>().PlayerNumber;
                Debug.Log($"PlayerNumber is: {playerNumber}");
                acornsEarned = earnAcorns.CalculateAcornsEarned(minigame, playerNumber);
                Debug.Log($"AcornsEarned: {acornsEarned}");
                StoreAcornsLocallyOnWinnerClient(sender, acornsEarned, minigame);
                break;
            case "WhackAMole":
                playerNumber = sender.identity.gameObject.GetComponent<HammerSpawnerOnline>().PlayerNumber;
                Debug.Log($"PlayerNumber is: {playerNumber}");
                acornsEarned = earnAcorns.CalculateAcornsEarned(minigame, playerNumber);
                Debug.Log($"AcornsEarned: {acornsEarned}");
                StoreAcornsLocallyOnWinnerClient(sender, acornsEarned, minigame);
                break;
            case "CowboyDuel":
                playerNumber = sender.identity.gameObject.GetComponent<PlayerShootOnline>().PlayerNumber;
                Debug.Log($"PlayerNumber is: {playerNumber}");
                acornsEarned = earnAcorns.CalculateAcornsEarned(minigame, playerNumber);
                Debug.Log($"AcornsEarned: {acornsEarned}");
                StoreAcornsLocallyOnWinnerClient(sender, acornsEarned, minigame);
                break;
        }
    }

    [TargetRpc]
    private void StoreAcornsLocallyOnWinnerClient(NetworkConnection target, int acornsEarned, string minigame)
    {
        acornsText.text = acornsEarned.ToString();
        
        switch (minigame)
        {
            case "RabbitPursuit":
                earnAcorns.AcornsRabbitPursuit(acornsEarned);
                database.AddPlayerRabbitPursuitGames();
                break;
            case "WhackAMole":
                earnAcorns.AcornsWhackAMole(acornsEarned);
                database.AddPlayerWhackAMoleGames();
                break;
            case "CowboyDuel":
                earnAcorns.AcornsCowboyDuel(acornsEarned);
                database.AddPlayerCowboyDuelGames();
                break;
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

    /*[TargetRpc]
    public void AnchorCatchButtonToPlayer(NetworkConnection target, int playerNumber)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject clientPlayer = Array.Find(players, player => player.GetComponent<PlayerMovementOnline>().playerNumber == playerNumber);
        catchButton.GetComponent<Button>().onClick.AddListener(clientPlayer.GetComponent<PlayerMovementOnline>().CatchButtonHandler);
    }*/

    public void AnchorCatchButtonToPlayer(UnityAction call)
    {
        catchButton.GetComponent<Button>().onClick.AddListener(call);
    }
}