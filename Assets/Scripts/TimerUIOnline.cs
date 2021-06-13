using System;
using System.Collections;
using Mirror;
using Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerUIOnline : NetworkBehaviour
{
    public event Action OnTimerEnd;
    
    [SerializeField]
    private Text timeText;

    [SerializeField] private GameObject sandClock;

    [SerializeField] private GameObject timeUI;
    //[SerializeField]
    //private float timeGameplay;

    private string formattedTime;
    private float time;
    private ITimer chronometer;

    [SerializeField]
    private PanelHandlerOnline panelHandler;

    [SyncVar(hook = nameof(OnTimeChanged))]
    private float serverTime;

    private void OnTimeChanged(float oldValue, float newValue)
    {
        UpdateTimerOnline(newValue);
    }
    
    /*
    private void Awake() 
    {
        //timeGameplay = 10f;
        Time.timeScale = 1;
        chronometer = ServiceLocator.Instance.GetService<ITimer>();
        chronometer.ResetTimer();
        UpdateTimer();
        //chronometer.StartTimer();
    }

    private void OnEnable()
    {
        chronometer.OnTimerOver += EnableResultPanelOnline;
    }

    private void OnDisable()
    {
        chronometer.OnTimerOver -= EnableResultPanelOnline;
    }

    // Update is called once per frame
    void Update()
    {
        chronometer.Tick(Time.deltaTime);
        UpdateTimer();
    }
    */

    private void FormatTime(int minutes, int seconds)
    {
        formattedTime = minutes.ToString("D1");
        formattedTime += ":";
        formattedTime += seconds.ToString("D2");
    }

    private void EnableResultPanelOnline()
    {
        if (!isServer) return;
        
        Debug.Log($"Am I client? {isClient}");
        Debug.Log($"Am I server? {isServer}");
        
        OnTimerEnd?.Invoke();
        
        DisableTimerOnClients();
        sandClock.GetComponent<Animator>().enabled = false;
        timeUI.SetActive(false);
        
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuitOnline":
                panelHandler.ShowRabbitPursuitPanel();
                break;
            case "WhackAMoleOnline":
                panelHandler.ShowWhackAMolePanel();
                break;
        }
        
        //Time.timeScale = 0;

        chronometer.OnTimerOver -= EnableResultPanelOnline;
        //EnableResultPanelOnClients();
    }

    
    [ClientRpc]
    private void DisableTimerOnClients()
    {
        /*
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuitOnline":
                panelHandler.ShowRabbitPursuitPanel();
                break;
            case "WhackAMoleOnline":
                panelHandler.ShowWhackAMolePanel();
                break;
        }
        //Time.timeScale = 0;
        */
        
        sandClock.GetComponent<Animator>().enabled = false;
        timeUI.SetActive(false);
    }
    

    private void UpdateTimer()
    {
        time = chronometer.GetCurrentTime();
        int minutes = (int) time / 60;
        int seconds = 0;

        if (Mathf.CeilToInt(time % 60) < 60)
        {
            seconds = Mathf.CeilToInt(time % 60);
        }
        else
        {
            minutes++;
        }
        //int seconds = (Mathf.CeilToInt(time % 60) < 60) : Mathf.CeilToInt(time % 60) ? 59;
        //Debug.Log($"Current seconds: {seconds}");
        FormatTime(minutes, seconds);
        //Debug.Log($"FormattedTime: {formattedTime}");
        timeText.text = formattedTime;
    }
    
    public void InitializeTimer()
    {
        //Debug.Log($"Ima server {isServer}");
        if (!isServer) return;
        
        chronometer = ServiceLocator.Instance.GetService<ITimer>();
        chronometer.ResetTimer();
        chronometer.OnTimerOver += EnableResultPanelOnline;
        UpdateTimerOnline(chronometer.GetCurrentTime());
    }
    
    public IEnumerator StartTimer()
    {
        //Debug.Log($"Ima server {isServer}");
        if (!isServer) yield break;
        
        float localTime = chronometer.GetCurrentTime();
        serverTime = localTime;
        
        while (localTime > 0)
        {
            chronometer.Tick(Time.deltaTime);
            localTime = chronometer.GetCurrentTime();
            serverTime = localTime;
            UpdateTimerOnline(localTime);

            yield return null;
        }
    }
    
    private void UpdateTimerOnline(float timeServer)
    {
        int minutes = (int) timeServer / 60;
        int seconds = 0;

        if (Mathf.CeilToInt(timeServer % 60) < 60)
        {
            seconds = Mathf.CeilToInt(timeServer % 60);
        }
        else
        {
            minutes++;
        }
        //int seconds = (Mathf.CeilToInt(time % 60) < 60) : Mathf.CeilToInt(time % 60) ? 59;
        //Debug.Log($"Current seconds: {seconds}");
        FormatTime(minutes, seconds);
        //Debug.Log($"FormattedTime: {formattedTime}");
        timeText.text = formattedTime;
    }
}
