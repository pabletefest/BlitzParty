using System;
using System.Collections;
using Mirror;
using Online.BinkyPursuit;
using UnityEngine;
using UnityEngine.UI;

namespace Online.CowboyDuel
{
    public class WinnerCheckerOnline : NetworkBehaviour
    {
        public event Action OnGameEnd;
    
        [SerializeField] private Text winnerLabel;

        [SerializeField] private PlayerShootOnline playerShoot;
        [SerializeField] private PlayerShootOnline player2Shoot;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator player2Animator;
        [SerializeField] private CountdownUIOnline gameCountdown;
        [SerializeField] private GameObject shootLabel;
        [SerializeField] private PlayersScoreOnline scoreController;

        private bool playerShot;
        private float playerTime = 2f;
    
        private bool player2Shot;
        private float player2Time = 2f;

        /*private void OnEnable()
        {
            playerShoot.OnShot += CheckSetup;
            enemyShoot.OnShot += CheckSetup;
        }


        private void OnDisable()
        {
            playerShoot.OnShot -= CheckSetup;
            enemyShoot.OnShot -= CheckSetup;
        }*/

        public void RecieveClientsData(float timeSinceReady, NetworkConnectionToClient sender)
        {
            int playerNumber = sender.identity.GetComponent<PlayerShootOnline>().PlayerNumber;
            //if (!isServer) return;
            //Debug.Log("Yeeeeeeeeeeeeeeeeeeeeeee im the server");
            if (playerNumber == 1)
            {
                playerShot = true;
                playerTime = timeSinceReady;
                Debug.Log($"Player 1 shoot time: {playerTime}");
            }
            else if (playerNumber == 2)
            {
                player2Shot = true;
                player2Time = timeSinceReady;
                Debug.Log($"Player 2 shoot time: {player2Time}");
            }
        }

        private void CheckRoundWinner()
        {
            if (playerShot && player2Shot)
            {
                Debug.Log($"Player 1 shot: {playerShot} && Player 2 shot {player2Shot}");
                Debug.Log($"Player 1 time: {playerTime} && Player 2 shot {player2Time}");
                if (playerTime < player2Time)
                {
                    scoreController.P1ScorePoints(1);
                    player2Animator.SetTrigger("Death");
                    winnerLabel.text = "RED POINT";
                    winnerLabel.gameObject.SetActive(true);
                }
                else if (playerTime > player2Time)
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
                player2Shot = false;

                CheckEndGame();
            }
        
        }

        // Update is called once per frame
        void Update()
        {
            ObtainPlayersReference();
            CheckRoundWinner();
        }

        private IEnumerator FinishRound()
        {
            shootLabel.SetActive(false);

            yield return new WaitForSeconds(3f);

            winnerLabel.gameObject.SetActive(false);

            playerAnimator.SetTrigger("RoundFinish");
            player2Animator.SetTrigger("RoundFinish");
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
                shootLabel.SetActive(false);
            }
        }

        private void ObtainPlayersReference()
        {
            if (!playerShoot)
            {
                GameObject player1 = GameObject.Find("Player 1");

                if (player1)
                {
                    playerShoot = player1.GetComponent<PlayerShootOnline>();
                    playerAnimator = player1.GetComponent<Animator>();
                    //playerShoot.OnShot += CheckSetup;
                }
                
            }

            if (!player2Shoot)
            {
                GameObject player2 = GameObject.Find("Player 2");

                if (player2)
                {
                    player2Shoot = player2.GetComponent<PlayerShootOnline>();
                    player2Animator = player2.GetComponent<Animator>();
                    //player2Shoot.OnShot += CheckSetup;
                }
            }
        }
    }
}
