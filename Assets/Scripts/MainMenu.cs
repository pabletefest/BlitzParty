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
    private OrientationManager orientationManager;

    [SerializeField]
    private Database database;

    [SerializeField]
    private Text acornLabel;

    [SerializeField]
    private Text usernameLabel;

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

    private int progress;

    private string nextScene;

    private void Awake()
    {
        acornLabel.text = database.LoadAcorns().ToString();
        usernameLabel.text = database.LoadUsername();
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
    }

    public void ShowFriendsTab()
    {
        friendsMenu.SetActive(true);
        friendsButton.enabled = false;
        shopButton.enabled = true;
        zoomyButton.enabled = true;
        mainButton.enabled = true;
        profileButton.enabled = true;
    }

    public void ShowProfileTab()
    {
        profileMenu.SetActive(true);
        profileButton.enabled = false;
        shopButton.enabled = true;
        zoomyButton.enabled = true;
        mainButton.enabled = true;
        friendsButton.enabled = true;
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
            settingsMenu.SetActive(true);
            ButtonsEnabled(false);
        }
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
            ButtonsEnabled(false);
        }
    }


    public void StartRabbitPursuitGame()
    {
        nextScene = "RabbitPursuit";
        StartTransition();
    }

    public void StartWhackAMoleGame()
    {
        nextScene = "Whack-a-Mole";
        StartTransition();
    }

    private void StartTransition()
    {
        orientationManager.ChangeScreenPortrait(false);
        transitionScreen.SetActive(true);
        progress = 0;
        InvokeRepeating("UpdateProgress", 0.05f, 0.05f);
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
