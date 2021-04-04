using UnityEngine;
using UnityEngine.UI;

public class PlayersScore : MonoBehaviour
{
    [SerializeField]
    private Text Player1Score;

    [SerializeField]
    private Text Player2Score;

    [SerializeField]
    private Text Player1ScorePanel;

    [SerializeField]
    private Text Player2ScorePanel;

    private ITimer chronometer;

    private void Awake() 
    {
        chronometer = ServiceLocator.Instance.GetService<ITimer>();    
    }

    private void OnEnable()
    {
        chronometer.OnTimerOver += UpdatePanelScore;
    }

    private void OnDisable()
    {
        chronometer.OnTimerOver -= UpdatePanelScore;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePanelScore()
    {
        Player1ScorePanel.text = Player1Score.text;
        Player2ScorePanel.text = Player2Score.text;
    }
}
