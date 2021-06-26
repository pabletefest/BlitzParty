using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using Online;
using Online.PlayFab;
using PlayFab;
using PlayFab.ClientModels;
using Services;

public class MainMenu : MonoBehaviour
{   

    //Tabs to show and hide when the buttons are clicked

    [SerializeField]
    public GameObject shopMenu;

    [SerializeField]
    public GameObject zoomyMenu;

    [SerializeField]
    public GameObject mainMenu;

    [SerializeField]
    public GameObject friendsMenu;

    [SerializeField]
    public GameObject profileMenu;

    [SerializeField]
    public GameObject settingsMenu;

    [SerializeField]
    private GameObject levelsMenu;


    //Buttons that need to be disabled when settings is active

    [SerializeField]
    public Button shopButton;

    [SerializeField]
    public Button zoomyButton;

    [SerializeField]
    public Button mainButton;

    [SerializeField]
    public Button friendsButton;

    [SerializeField]
    public Button profileButton;

    [SerializeField]
    public Button settingsButton;

    [SerializeField]
    public Button playButton;

    [SerializeField]
    private Button rabbitPursuitButton;

    [SerializeField]
    private Button whackamoleButton;

    //Other fields
    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private OrientationManager orientationManager;

    [SerializeField]
    private Database database;

    [SerializeField]
    private Text acornLabel;

    [SerializeField]
    private InventoryManager inventoryManager;

    [SerializeField]
    private GameObject headItem;

    [SerializeField]
    private GameObject bodyItem;

    [SerializeField]
    private GameObject lowerItem;

    [SerializeField]
    private GameObject transitionScreen;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text percentageText;

    [SerializeField]
    private Text messageText;

    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject fakeTransition;

    [SerializeField]
    private ProfileMenuHandler profileMenuHandler;

    [SerializeField]
    private Button allGamesButton;

    [SerializeField]
    private Button battleButton;

    [SerializeField]
    private Button playBattleButton;

    [SerializeField]
    private GameObject minigameMenu;

    [SerializeField]
    private GameObject battleMenu;

    [SerializeField]
    private GameObject animationMusic;

    [SerializeField]
    private GameObject animationMute;

    [SerializeField]
    private BattleModeHandler battleModeHandler;

    [SerializeField]
    private GameObject resultsPanel;

    [SerializeField]
    private Image resultImage;

    [SerializeField]
    private Image trophyImage;

    [SerializeField]
    private Text player1Score;

    [SerializeField]
    private Text player2Score;

    [SerializeField]
    private Sprite victoryImage;

    [SerializeField]
    private Sprite defeatImage;

    [SerializeField]
    private Sprite goldTrophy;

    [SerializeField]
    private Sprite silverTrophy;

    [SerializeField]
    private Button volumeButton;

    [SerializeField]
    private Button creditsButton;

    [SerializeField]
    private GameObject volumeMenu;

    [SerializeField]
    private GameObject creditsMenu;

    [SerializeField] private GameObject loadingScreenPrefab;
    [SerializeField] private GameObject fakeTransitionPrefab;

    private int progress;

    private string nextScene;

    private Dictionary<string, string> userData;
    private string playFabId;

    private static bool isFirstLoad = true;

    private void Awake()
    {
        Time.timeScale = 1;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme();

        acornLabel = GameObject.Find("AcornLabel").GetComponent<Text>();
        
        //acornLabel.text = database.LoadAcorns().ToString();
        //musicSlider.value = database.LoadMusicVolume();
        //sfxSlider.value = database.LoadSFXVolume();
        
        if (!PlayFabClientAPI.IsClientLoggedIn() || database.GetAccountActiveToken() == 1)
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                Username = database.GetUsername(),
                Password = database.GetPassword()
            }, result =>
                {
                    Debug.Log($"LoggedIn on MainMenu");

                    if (isFirstLoad)
                    {
                        PlayFabLogin.SessionTicket = result.SessionTicket;
                        PlayFabLogin.PlayFabId = result.PlayFabId;
                        Debug.Log(PlayFabLogin.PlayFabId);
                        
                        CloudStoragePlayFab cloudStorage = new CloudStoragePlayFab();
                        cloudStorage.OnDataReceived += UpdateUserData;
                        cloudStorage.GetUserData(PlayFabLogin.PlayFabId);
                        
                        isFirstLoad = false;
                    }
                },
                error => Debug.LogError($"Couldn't login on MainMenu: {error.GenerateErrorReport()}"));
        }

        if (PlayFabLogin.PlayFabId != default)
        {
            CloudStoragePlayFab cloudStorageData = new CloudStoragePlayFab();
            cloudStorageData.OnDataReceived += UpdateUserData;
            cloudStorageData.GetUserData(PlayFabLogin.PlayFabId);
        }
        else
        {
            acornLabel.text = database.LoadAcorns().ToString();
        }

        if (acornLabel.text == "-")
            acornLabel.text = database.LoadAcorns().ToString();

        //acornLabel.text = database.LoadAcorns().ToString();

        /*if (!database.IsBattleMode())
        {
            orientationManager.ChangeScreenPortrait(true);
            //acornLabel.text = database.LoadAcorns().ToString();
            SetZoomyItems();
            //musicSlider.value = database.LoadMusicVolume();
            //sfxSlider.value = database.LoadSFXVolume();

            
            if (database.LoadCurrentBattleStage() == 3)
            {
                ShowResultsPanel();
                database.ResetMinigames();
            }
        }
        else 
        {
            nextScene = database.LoadCurrentBattleMinigame();
            StartTransition();
        }*/
    }

    // private void Start()
    // {
    //     NetworkTypeChecker.Instance.SelectNetworkType(0);
    // }

    private void UpdateUserData(Dictionary<string, string> cloudUserData)
    {
        database.SaveAcorns(int.Parse(cloudUserData["Acorns"]));
        Debug.Log($"AcornsLabel: {acornLabel}");
        if (acornLabel != null && acornLabel.text != null)
        {
            acornLabel.text = cloudUserData["Acorns"];
        }
        musicSlider.value = float.Parse(cloudUserData["MusicVolume"]);
        sfxSlider.value = float.Parse(cloudUserData["SFXVolume"]);
        playFabId = PlayFabLogin.PlayFabId ?? cloudUserData["PlayFabId"] ?? database.GetPlayFabId();
        profileMenuHandler.UpdateProfileData(cloudUserData["Username"]);
        userData = cloudUserData;
    }

    private void ShowResultsPanel() 
    {
        StartButtonsEnabled(false);
        int p1Score = database.LoadPlayer1BattleWins();
        int p2Score = database.LoadPlayer2BattleWins();
        if (p1Score > p2Score)
        {
            resultImage.sprite = victoryImage;
            trophyImage.sprite = goldTrophy;
        }
        else
        {
            resultImage.sprite = defeatImage;
            trophyImage.sprite = silverTrophy;
        }
        player1Score.text = p1Score.ToString();
        player2Score.text = p2Score.ToString();
        resultsPanel.SetActive(true);
    }

    public void CloseResultsPanel()
    {
        resultsPanel.SetActive(false);
        StartButtonsEnabled(true);
    }

    public void HideTabs() 
    {
        shopMenu.SetActive(false);
        zoomyMenu.SetActive(false);
        mainMenu.SetActive(false);
        friendsMenu.SetActive(false);
        profileMenu.SetActive(false);
    }

    private void ResetButtonsSprites()
    {
        shopButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Shop Button");
        zoomyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Zoomy Button");
        mainButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Main Button");
        friendsButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Friends Button");
        profileButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Profile Button");
    }

    private void ButtonsEnabled(bool enabled)
    {
        shopButton.enabled = enabled;
        zoomyButton.enabled = enabled;
        mainButton.enabled = enabled;
        friendsButton.enabled = enabled;
        profileButton.enabled = enabled;
    }

    public void ShowShopTab()
    {
        ResetButtonsSprites();
        shopButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Shop Button Selected");

        ButtonsEnabled(true);
        shopButton.enabled = false;

        shopMenu.SetActive(true);

        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowZoomyTab()
    {
        ResetButtonsSprites();
        zoomyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Zoomy Button Selected");

        ButtonsEnabled(true);
        zoomyButton.enabled = false;

        inventoryManager.UpdateItemsList();

        zoomyMenu.SetActive(true);
        
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowMainTab()
    {
        ResetButtonsSprites();
        mainButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Main Button Selected");

        ButtonsEnabled(true);
        mainButton.enabled = false;

        SetZoomyItems();

        mainMenu.SetActive(true);

        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowFriendsTab()
    {
        ResetButtonsSprites();
        friendsButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Friends Button Selected");

        ButtonsEnabled(true);
        friendsButton.enabled = false;

        friendsMenu.SetActive(true);
        
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowProfileTab()
    {
        ResetButtonsSprites();
        profileButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ButtonMenu/Profile Button Selected");

        ButtonsEnabled(true);
        profileButton.enabled = false;
        
        profileMenuHandler.UpdateProfileData(userData?["Username"]);

        profileMenu.SetActive(true);
        
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void SettingsTab()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            StartButtonsEnabled(true);
        }
        else
        {
            musicSlider.value = database.LoadMusicVolume();
            sfxSlider.value = database.LoadSFXVolume();
            if (musicSlider.value < 0.001)
            {
                animationMusic.SetActive(false);
                animationMute.SetActive(true);
            }
            else
            {
                animationMusic.SetActive(true);
                animationMute.SetActive(false);
            }
            StartButtonsEnabled(false);
            settingsMenu.SetActive(true);
        }
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void volumeSettingsButtonHandler()
    {
        volumeButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Settings/VolumeButtonSelected");
        creditsButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Settings/CreditsButton");
        volumeMenu.SetActive(true);
        creditsMenu.SetActive(false);
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void creditsButtonHandler()
    {
        volumeButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Settings/VolumeButton");
        creditsButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Settings/CreditsButtonSelected");
        volumeMenu.SetActive(false);
        creditsMenu.SetActive(true);
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void StartButtonsEnabled(bool enabled)
    {
        shopButton.interactable = enabled;
        zoomyButton.interactable = enabled;
        mainButton.interactable = enabled;
        friendsButton.interactable = enabled;
        profileButton.interactable = enabled;
        settingsButton.interactable = enabled;
        playButton.interactable = enabled;
    }

    public void StartButtonHandler()
    {
        if (levelsMenu.activeSelf)
        {
            levelsMenu.SetActive(false);
            StartButtonsEnabled(true);
        }
        else
        {
            levelsMenu.SetActive(true);
            minigameMenu.SetActive(true);
            battleMenu.SetActive(false);
            allGamesButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption1Selected");
            battleButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption2");
            StartButtonsEnabled(false);
        }
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void allMinigamesButtonHandler()
    {
        minigameMenu.SetActive(true);
        battleMenu.SetActive(false);
        allGamesButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption1Selected");
        battleButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption2");
    }

    public void battleButtonHandler()
    {
        minigameMenu.SetActive(false);
        battleMenu.SetActive(true);
        allGamesButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption1");
        battleButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption2Selected");
    }

    public void battlePlayButtonHandler()
    {
        battleModeHandler.StartBattle();
    }

    public void StartRabbitPursuitGame()
    {
        nextScene = "RabbitPursuit";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        StartTransition();
    }
    
    public void StartRabbitPursuitGameOnline()
    {
        nextScene = "RabbitPursuitOnline";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        NetworkTypeChecker.Instance.SelectNetworkType(NetworkType.LAN);
        StartTransition();
    }

    public void StartWhackAMoleGame()
    {
        nextScene = "Whack-a-Mole";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        StartTransition();
    }
    
    public void StartWhackAMoleGameOnline()
    {
        nextScene = "WhackAMoleOnline";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        NetworkTypeChecker.Instance.SelectNetworkType(NetworkType.LAN);
        StartTransition();
    }
    
    public void StartCowboyDuelGame()
    {
        nextScene = "CowboyDuel";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        StartTransition();
    }
    
    public void StartCowboyDuelGameOnline()
    {
        nextScene = "CowboyDuelOnline";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        NetworkTypeChecker.Instance.SelectNetworkType(NetworkType.LAN);
        StartTransition();
    }

    public void StartNextMinigameOnline(string minigame)
    {
        transitionScreen = Instantiate(loadingScreenPrefab);
        transitionScreen.transform.SetParent(GameObject.Find("Canvas").transform);
        transitionScreen.SetActive(false);
        fakeTransition = Instantiate(fakeTransitionPrefab);
        fakeTransition.transform.SetParent(GameObject.Find("Canvas").transform);
        fakeTransition.SetActive(false);
        nextScene = minigame;
        StartTransition();
    }

    private void StartTransition()
    {
        UpdateMessageText();
        orientationManager.ChangeScreenPortrait(false);
        transitionScreen.SetActive(true);
        panel.SetActive(false);
        progress = 0;
        InvokeRepeating("UpdateProgress", 0.05f, 0.05f);
    }

    private void UpdateMessageText()
    {
        switch (nextScene)
        {
            case "RabbitPursuit":
                messageText.text = "A plague of Binkies has been found in the forest clearing.  What a great opportunity to have a little duel. Let's see who catches the most Binkies!";
                fakeTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>("FakeTransition/RabbitPursuitTransition");
                break;
            case "Whack-a-Mole":
                messageText.text = "Something has made the moles act aggressive. Smack them with the hammer when they come out of their burrows but watch out for the Zoomies!";
                fakeTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>("FakeTransition/WhackAMoleTransition");
                break;
            case "CowboyDuel":
                messageText.text = "Travel to the Wild West to take part in an epic duel. Wait until the signal and shoot before your opponent. Become the fastest Cowboy alive!";
                fakeTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>("FakeTransition/CowboyDuelTransition");
                break;
            case "RabbitPursuitOnline":
                messageText.text = "A plague of Binkies has been found in the forest clearing.  What a great opportunity to have a little duel. Let's see who catches the most Binkies!";
                fakeTransition.GetComponent<Image>().sprite = Resources.Load<Sprite>("FakeTransition/RabbitPursuitTransition");
                break;
        }
    }

    void UpdateProgress()
    {
        if (progress < 100)
        {
            progress++;
            slider.value = progress;
            percentageText.text = progress.ToString() + " %";
        }
        else 
        {
            CancelInvoke("UpdateProgress");
            transitionScreen.SetActive(false);
            fakeTransition.SetActive(true);
            SceneManager.LoadScene(nextScene);
        }
    }

    private void SetZoomyItems()
    {
        if (!database.LoadHeadPiece().Equals("none"))
        {
            headItem.SetActive(true);
            headItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Icons/" + database.LoadHeadPiece());
        }
        else 
        {
            headItem.SetActive(false);
        }
        if (!database.LoadBodyPiece().Equals("none"))
        {
            bodyItem.SetActive(true);
            bodyItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Icons/" + database.LoadBodyPiece());
        }
        else
        {
            bodyItem.SetActive(false);
        }
        if (!database.LoadLowerPiece().Equals("none"))
        {
            lowerItem.SetActive(true);
            lowerItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Icons/" + database.LoadLowerPiece());
        }
        else
        {
            lowerItem.SetActive(false);
        }
    }

}
