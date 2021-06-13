using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    
    [SerializeField]
    private GameObject transitionScreen;
    
    [SerializeField]
    private GameObject fakeTransition;

    [SerializeField]
    private Slider slider;
    
    [SerializeField]
    private Text percentageText;
    
    [SerializeField]
    private Text messageText;
    
    private int progress;
    private string nextScene;

    [SerializeField] private GameObject sandClock;


    public void StartNextMinigameOnline(string minigame)
    {
        nextScene = minigame;
        StartTransition();
    }

    private void StartTransition()
    {
        UpdateMessageText();
        //orientationManager.ChangeScreenPortrait(false);
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
}