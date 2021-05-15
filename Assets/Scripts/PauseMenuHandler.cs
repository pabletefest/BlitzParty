using RabbitPursuit;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WhackAMole;

public class PauseMenuHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    [SerializeField]
    private OrientationManager orientationManager;

    public void ShowRabbitPursuitPauseMenu()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        joystick.GetComponent<Canvas>().enabled = false;
        catchButton.SetActive(false);
    }

    public void ShowWhackAMolePauseMenu()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        DestroyRemainingHammers();
    }

    public void ShowCowboyDuelPauseMenu()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideRabbitPursuitPauseMenu()
    {
        pausePanel.SetActive(false);
        joystick.GetComponent<Canvas>().enabled = true;
        catchButton.SetActive(true);
        Time.timeScale = 1;
    }


    public void HideWhackAMolePauseMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
    public void HideCowboyDuelPauseMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void MenuButtonHandler()
    {
        orientationManager.ChangeScreenPortrait(true);
        SceneManager.LoadScene("MainMenu");
        ServiceLocator.Instance.GetService<IObjectPooler>().ClearAllPools();
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMainTheme();
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
