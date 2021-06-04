using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class PlayersScoreOnline : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateOnClientsP1))]
    private int p1Score;

    [SyncVar(hook = nameof(UpdateOnClientsP2))]
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

    private void UpdateOnClientsP1(int oldValue, int newValue)
    {
        Player1Score.text = newValue.ToString();
    }
    
    private void UpdateOnClientsP2(int oldValue, int newValue)
    {
        Player2Score.text = newValue.ToString();
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
        if (!isServer) return;
        
        if (playerIdentity == 1)
        {
            p1Score += amount;
            if (p1Score < 0) p1Score = 0;
            UpdateScore();
        }   
        else if(playerIdentity == 2)
        {
            p2Score += amount;
            if (p2Score < 0) p2Score = 0;
            UpdateScore();
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
        else return Results.PLAYER2WIN;
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