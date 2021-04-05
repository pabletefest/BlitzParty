using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("RabbitPursuit", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MainMenu");
    }

}
