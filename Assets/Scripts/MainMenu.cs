using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Services;

public class MainMenu : MonoBehaviour
{   
    //public static event Action OnRabbitPursuitLoaded;

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

    private void Awake()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme(); 
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
    }

    public void ShowZoomyTab()
    {
        zoomyMenu.SetActive(true);
    }

    public void ShowMainTab()
    {
        mainMenu.SetActive(true);
    }

    public void ShowFriendsTab()
    {
        friendsMenu.SetActive(true);
    }

    public void ShowProfileTab()
    {
        profileMenu.SetActive(true);
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
        SceneManager.LoadScene("RabbitPursuit");
        //AsyncOperation asyncScene = SceneManager.LoadSceneAsync("RabbitPursuit", LoadSceneMode.Additive);
        //asyncScene.allowSceneActivation = true;
        //SceneManager.UnloadSceneAsync("MainMenu");
        //StartCoroutine(nameof(ActivateRabbitPursuitScene), asyncScene);
    }

    /*
    private IEnumerator ActivateRabbitPursuitScene(AsyncOperation asyncSceneLoad)
    {
        while(!asyncSceneLoad.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("RabbitPursuit"));
        OnRabbitPursuitLoaded?.Invoke();
    }
    */
}
