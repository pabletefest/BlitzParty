using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class PlayersScoreOnline : MonoBehaviour
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
        Player1ScorePanel.text = p1Score.ToString();
        Player2ScorePanel.text = p2Score.ToString();

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

    public void PlayerScorePoints(int amount, int playerIdentity) 
    {
        if (playerIdentity == 1)
        {
            P1ScorePoints(amount);
            if (p1Score < 0) p1Score = 0;
        }   
        else if(playerIdentity == 2)
        {
            P2ScorePoints(amount);
            if (p2Score < 0) p2Score = 0;
        }
    }

    public void ResetWhackAMoleScore()
    {
        p1Score = 0;
        p2Score = 80;
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

    public int GetP2Score()
    {
        return p2Score;
    }
}