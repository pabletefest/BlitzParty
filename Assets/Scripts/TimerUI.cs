using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private GameObject joysStick;

    //[SerializeField]
    //private float timeGameplay;

    private string formattedTime;
    private float time;
    private ITimer chronometer;

    private void Awake() 
    {
        //timeGameplay = 10f;
        chronometer = ServiceLocator.Instance.GetService<ITimer>(); 
        chronometer.StartTimer();   
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
        resultPanel.SetActive(true);
        ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("ArrowImpact");
        joysStick.SetActive(false);
    }

    private void UpdateTimer()
    {
        time = chronometer.GetCurrentTime();
        int minutes = (int) time / 60;
        int seconds = Mathf.CeilToInt(time % 60);
        //Debug.Log($"Current seconds: {seconds}");
        FormatTime(minutes, seconds);
        //Debug.Log($"FormattedTime: {formattedTime}");
        timeText.text = formattedTime;
    }
}
