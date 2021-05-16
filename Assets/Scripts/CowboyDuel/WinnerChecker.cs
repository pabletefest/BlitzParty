using System;
using CowboyDuel;
using UnityEngine;
using UnityEngine.UI;

public class WinnerChecker : MonoBehaviour
{
    public event Action OnGameEnd;
    
    [SerializeField] private Text p1Score;
    [SerializeField] private Text p2Score;
    
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private EnemyShoot enemyShoot;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator enemyAnimator;

    private bool playerShot;
    private float playerTime;
    
    private bool enemyShot;
    private float enemyTime;

    private void OnEnable()
    {
        playerShoot.OnShot += CheckSetup;
        enemyShoot.OnShot += CheckSetup;
    }


    private void OnDisable()
    {
        playerShoot.OnShot -= CheckSetup;
        enemyShoot.OnShot -= CheckSetup;
    }

    private void CheckSetup(string tag, float timeSinceReady)
    {
        if (tag == "Player")
        {
            playerShot = true;
            playerTime = timeSinceReady;
        }
        else if (tag == "Enemy")
        {
            enemyShot = true;
            enemyTime = timeSinceReady;
        }
    }

    private void CheckRoundWinner()
    {
        if (playerShot && enemyShot)
        {
            if (playerTime < enemyTime)
            {
                int newPoint = Int32.Parse(p1Score.text) + 1;
                p1Score.text = newPoint.ToString();
                enemyAnimator.SetTrigger("Death");
            }
            else if (playerTime > enemyTime)
            {
                int newPoint = Int32.Parse(p2Score.text) + 1;
                p2Score.text = newPoint.ToString();
                playerAnimator.SetTrigger("Death");
            }
            else
            {
                int randomWinner = UnityEngine.Random.Range(1, 3);

                if (randomWinner == 1)
                {
                    int newPoint = Int32.Parse(p1Score.text) + 1;
                    p1Score.text = newPoint.ToString();
                }
                else
                {
                    int newPoint = Int32.Parse(p2Score.text) + 1;
                    p2Score.text = newPoint.ToString();
                }
            }
            playerShot = false;
            enemyShot = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        int player1Score = Int32.Parse(p1Score.text);
        int player2Score = Int32.Parse(p2Score.text);

        if (player1Score == 2 || player2Score == 2)
        {
            OnGameEnd?.Invoke();
            return;
        }
        
        CheckRoundWinner();
    }
}
