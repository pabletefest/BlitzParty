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

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject pauseButton;

    public void ShowRabbitPursuitPauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        musicSlider.value = database.LoadMusicVolume();
        sfxSlider.value = database.LoadSFXVolume();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        joystick.GetComponent<Canvas>().enabled = false;
        catchButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void ShowWhackAMolePauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        musicSlider.value = database.LoadMusicVolume();
        sfxSlider.value = database.LoadSFXVolume();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        DestroyRemainingHammers();
        pauseButton.SetActive(false);
    }

    public void ShowCowboyDuelPauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        musicSlider.value = database.LoadMusicVolume();
        sfxSlider.value = database.LoadSFXVolume();
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        pauseButton.SetActive(false);
    }

    public void HideRabbitPursuitPauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        pausePanel.SetActive(false);
        joystick.GetComponent<Canvas>().enabled = true;
        catchButton.SetActive(true);
        Time.timeScale = 1;
        pauseButton.SetActive(true);
    }


    public void HideWhackAMolePauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        pauseButton.SetActive(true);
    }
    public void HideCowboyDuelPauseMenu()
    {
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        pauseButton.SetActive(true);
    }

    public void MenuButtonHandler()
    {
        database.SetIsBattleMode(false);
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ButtonClickSFX");
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
