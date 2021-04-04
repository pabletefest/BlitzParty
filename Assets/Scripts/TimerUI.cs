using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private float timeGameplay;

    private string formattedTime;
    private float time;
    private ITimer chronometer;

    private void Awake() 
    {
        timeGameplay = 10f;
        chronometer = ServiceLocator.Instance.GetService<ITimer>();    
    }

    private void OnEnable()
    {
        chronometer.OnTimerOver += EnableResultPanel;
    }

    private void OnDisable()
    {
        chronometer.OnTimerOver -= EnableResultPanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        chronometer.SetTimeInSeconds(timeGameplay);
        chronometer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        time = chronometer.GetCurrentTime();
        int minutes = (int) time / 60;
        int seconds = (int) time % 60;
        //Debug.Log($"Current seconds: {seconds}");
        FormatTime(minutes, seconds);
        //Debug.Log($"FormattedTime: {formattedTime}");
        timeText.text = formattedTime;
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
    }
}
