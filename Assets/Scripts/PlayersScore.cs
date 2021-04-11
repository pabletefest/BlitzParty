using UnityEngine;
using UnityEngine.UI;

public class PlayersScore : MonoBehaviour
{

    private int p1Score;
    private int p2Score;

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
        chronometer.OnTimerOver += UpdateScore;
    }

    private void OnDisable()
    {
        chronometer.OnTimerOver -= UpdateScore;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
        UpdateScore();
    }

    private void UpdateScore()
    {
        Player1Score.text = p1Score.ToString();
        Player2Score.text = p2Score.ToString();
        Player1ScorePanel.text = Player1Score.text;
        Player2ScorePanel.text = Player2Score.text;

    }

    public void P1ScorePoint()
    {
        p1Score++;
        UpdateScore();
    }

    public void P2ScorePoint()
    {
        p2Score++;
        UpdateScore();
    }

    public void ResetScore()
    {
        p1Score = 0;
        p2Score = 0;
        UpdateScore();
    }

    public RabbitPursuitResults FindWinner()
    {
        if (p1Score > p2Score) return RabbitPursuitResults.PLAYER1WIN;
        else if (p1Score == p2Score) return RabbitPursuitResults.DRAW;
        else return RabbitPursuitResults.PLAYER2WIN;
    }
}
