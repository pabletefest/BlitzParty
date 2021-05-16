using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
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

    private int progress;

    private string nextScene;

    private void Awake()
    {
        Time.timeScale = 1;
        acornLabel.text = database.LoadAcorns().ToString();
        SetZoomyItems();
    }

    public void HideTabs() 
    {
        shopMenu.SetActive(false);
        zoomyMenu.SetActive(false);
        mainMenu.SetActive(false);
        friendsMenu.SetActive(false);
        profileMenu.SetActive(false);
    }

    public void ShowShopTab()
    {
        shopMenu.SetActive(true);
        shopButton.enabled = false;
        zoomyButton.enabled = true;
        mainButton.enabled = true;
        friendsButton.enabled = true;
        profileButton.enabled = true;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowZoomyTab()
    {
        zoomyMenu.SetActive(true);
        inventoryManager.UpdateItemsList();
        zoomyButton.enabled = false;
        shopButton.enabled = true;
        mainButton.enabled = true;
        friendsButton.enabled = true;
        profileButton.enabled = true;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowMainTab()
    {
        mainMenu.SetActive(true);
        SetZoomyItems();
        mainButton.enabled = false;
        shopButton.enabled = true;
        zoomyButton.enabled = true;
        friendsButton.enabled = true;
        profileButton.enabled = true;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowFriendsTab()
    {
        friendsMenu.SetActive(true);
        friendsButton.enabled = false;
        shopButton.enabled = true;
        zoomyButton.enabled = true;
        mainButton.enabled = true;
        profileButton.enabled = true;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ShowProfileTab()
    {
        profileMenu.SetActive(true);
        profileMenuHandler.UpdateProfileData();
        profileButton.enabled = false;
        shopButton.enabled = true;
        zoomyButton.enabled = true;
        mainButton.enabled = true;
        friendsButton.enabled = true;
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void SettingsTab()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
            ButtonsEnabled(true);
        }
        else
        {
            musicSlider.value = database.LoadMusicVolume();
            sfxSlider.value = database.LoadSFXVolume();
            settingsMenu.SetActive(true);
            ButtonsEnabled(false);
        }
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
    }

    public void ButtonsEnabled(bool enabled)
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
            ButtonsEnabled(true);
        }
        else
        {
            levelsMenu.SetActive(true);
            minigameMenu.SetActive(true);
            battleMenu.SetActive(false);
            allGamesButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption1Selected");
            battleButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("PlayMenu/ChooseGameMenuOption2");
            ButtonsEnabled(false);
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
        
    }

    public void StartRabbitPursuitGame()
    {
        nextScene = "RabbitPursuit";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        StartTransition();
    }

    public void StartWhackAMoleGame()
    {
        nextScene = "Whack-a-Mole";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        StartTransition();
    }
    
    public void StartCowboyDuelGame()
    {
        nextScene = "CowboyDuel";
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
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
        }
    }

    void UpdateProgress()
    {
        if (progress < 100)
        {
            progress++;
            slider.value = progress;
            percentageText.text = progress.ToString() + " %";
            Debug.Log(slider.value);
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
