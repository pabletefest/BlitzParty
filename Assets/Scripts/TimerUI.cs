using Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject joysStick;

    //[SerializeField]
    //private float timeGameplay;

    private string formattedTime;
    private float time;
    private ITimer chronometer;

    [SerializeField]
    private PanelHandler panelHandler;

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
        chronometer.OnTimerOver += EnableResultPanel;
    }

    private void OnDisable()
    {
        chronometer.OnTimerOver -= EnableResultPanel;
    }

    // Update is called once per frame
    void Update()
    {
        chronometer.Tick(Time.deltaTime);
        UpdateTimer();
    }

    private void FormatTime(int minutes, int seconds)
    {
        formattedTime = minutes.ToString("D1");
        formattedTime += ":";
        formattedTime += seconds.ToString("D2");
    }

    private void EnableResultPanel()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuit":
                panelHandler.ShowRabbitPursuitPanel();
                break;
            case "Whack-a-Mole":
                panelHandler.ShowWhackAMolePanel();
                break;
        }
        Time.timeScale = 0;
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
}
