using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CowboyDuel
{
    public class WinnerChecker : MonoBehaviour
    {
        public event Action OnGameEnd;
    
        [SerializeField] private Text winnerLabel;

        [SerializeField] private PlayerShoot playerShoot;
        [SerializeField] private EnemyShoot enemyShoot;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private CountdownUI gameCountdown;
        [SerializeField] private GameObject shootLabel;
        [SerializeField] private PlayersScore scoreController;

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
                    scoreController.P1ScorePoints(1);
                    enemyAnimator.SetTrigger("Death");
                    winnerLabel.text = "RED POINT";
                    winnerLabel.gameObject.SetActive(true);
                }
                else if (playerTime > enemyTime)
                {
                    scoreController.P2ScorePoints(1);
                    playerAnimator.SetTrigger("Death");
                    winnerLabel.text = "BLUE POINT";
                    winnerLabel.gameObject.SetActive(true);
                }
                else
                {
                    int randomWinner = UnityEngine.Random.Range(1, 3);

                    if (randomWinner == 1)
                    {
                        scoreController.P1ScorePoints(1);
                    }
                    else
                    {
                        scoreController.P2ScorePoints(1);
                    }
                }
                playerShot = false;
                enemyShot = false;

                CheckEndGame();
            }
        
        }

        // Update is called once per frame
        void Update()
        {
            CheckRoundWinner();
        }

        private IEnumerator FinishRound()
        {
            shootLabel.SetActive(false);

            yield return new WaitForSeconds(3f);

            winnerLabel.gameObject.SetActive(false);

            playerAnimator.SetTrigger("RoundFinish");
            enemyAnimator.SetTrigger("RoundFinish");
        }

        private void CheckEndGame()
        {
            int player1Score = scoreController.GetP1Score();
            int player2Score = scoreController.GetP2Score();

            if (player1Score < 2 && player2Score < 2)
            {
                StartCoroutine(FinishRound());
            } 
            else if (player1Score == 2 || player2Score == 2)
            {
                OnGameEnd?.Invoke();
                return;
            }
        }
    }
}
