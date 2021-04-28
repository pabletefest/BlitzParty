using Services;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "RabbitPursuit":
                ResetRabbitPursuitScore();
                break;
            case "Whack-a-Mole":
                ResetWhackAMoleScore();
                break;
        }
    }

    private void UpdateScore()
    {
        Player1Score.text = p1Score.ToString();
        Player2Score.text = p2Score.ToString();
        Player1ScorePanel.text = Player1Score.text;
        Player2ScorePanel.text = Player2Score.text;

    }

    public void P1ScorePoints(int points)
    {
        p1Score += points;
        UpdateScore();
    }

    public void P1SubstractPoints(int points)
    {
        p1Score -= points;
        if (p1Score < 0) p1Score = 0;
        UpdateScore();
    }

    public void P2ScorePoints(int points)
    {
        p2Score += points;
        UpdateScore();
    }

    public void ResetRabbitPursuitScore()
    {
        p1Score = 0;
        p2Score = 0;
        UpdateScore();
    }

    public void ResetWhackAMoleScore()
    {
        p1Score = 0;
        p2Score = 50;
        UpdateScore();
    }

    public Results FindWinner()
    {
        if (p1Score > p2Score) return Results.PLAYER1WIN;
        else if (p1Score == p2Score) return Results.DRAW;
        else return Results.PLAYER1LOSE;
    }

    public int GetP1Score()
    {
        return p1Score;
    }
}
