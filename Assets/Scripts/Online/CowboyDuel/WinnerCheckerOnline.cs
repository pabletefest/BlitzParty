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

        [Command(requiresAuthority = false)]
        public void RecieveClient1Data(float timeSinceReady)
        {
            if (!isServer) return;

            playerShot = true;
            playerTime = timeSinceReady;
            Debug.Log($"Player 1 shoot time: {playerTime}");
        }
        
        [Command(requiresAuthority = false)]
        public void RecieveClient2Data(float timeSinceReady)
        {
            if (!isServer) return;

            player2Shot = true;
            player2Time = timeSinceReady;
            Debug.Log($"Player 2 shoot time: {player2Time}");
        }

        private void CheckRoundWinner()
        {
            if (playerShot && player2Shot)
            {
                Debug.Log($"Player 1 shot: {playerShot} && Player 2 shot {player2Shot}");
                Debug.Log($"Player 1 time: {playerTime} && Player 2 shot {player2Time}");
                if (playerTime < player2Time)
                {
                    scoreController.PlayerScorePoints(1, 1);
                    //player2Animator.SetTrigger("Death");
                    RpcSetDeathAnimationPlayer(player2Animator.gameObject);
                    // winnerLabel.text = "RED POINT";
                    // winnerLabel.gameObject.SetActive(true);
                    RpcShowWinnerOnClients("RED POINT");
                }
                else if (playerTime > player2Time)
                {
                    scoreController.PlayerScorePoints(1, 2);
                    //playerAnimator.SetTrigger("Death");
                    RpcSetDeathAnimationPlayer(playerAnimator.gameObject);
                    // winnerLabel.text = "BLUE POINT";
                    // winnerLabel.gameObject.SetActive(true);
                    RpcShowWinnerOnClients("BLUE POINT");
                }
                else
                {
                    int randomWinner = UnityEngine.Random.Range(1, 3);

                    if (randomWinner == 1)
                    {
                        scoreController.PlayerScorePoints(1, 1);
                        //player2Animator.SetTrigger("Death");
                        RpcSetDeathAnimationPlayer(player2Animator.gameObject);
                    }
                    else
                    {
                        scoreController.PlayerScorePoints(1, 2);
                        //playerAnimator.SetTrigger("Death");
                        RpcSetDeathAnimationPlayer(playerAnimator.gameObject);
                    }
                }
                playerShot = false;
                player2Shot = false;

                CheckEndGame();
            }
        
        }

        [ClientRpc]
        private void RpcShowWinnerOnClients(string winner)
        {
            winnerLabel.text = winner;
            winnerLabel.gameObject.SetActive(true);
        }
        
        [ClientRpc]
        private void RpcSetDeathAnimationPlayer(GameObject player)
        {
            Debug.Log(player);
            //player.GetComponent<Animator>().SetTrigger("Death");
            player.GetComponent<NetworkAnimator>().SetTrigger("Death");
        }

        // Update is called once per frame
        void Update()
        {
            //if (!isSetReference)
            ObtainPlayersReference(); //For client animations
            
            if (!isServer) return;
            //Debug.Log($"Player 1 shot: {playerShot} && Player 2 shot {player2Shot}");
            //ObtainPlayersReference();
            CheckRoundWinner();
        }

        private void FinishRound()
        {
            RpcRestartRoundOnClients(playerAnimator.gameObject, player2Animator.gameObject);
            // shootLabel.SetActive(false);
            //
            // yield return new WaitForSeconds(3f);
            //
            // winnerLabel.gameObject.SetActive(false);

            //playerAnimator.SetTrigger("RoundFinish");
            //player2Animator.SetTrigger("RoundFinish");
        }
        
        private IEnumerator FinishRoundOnClients(GameObject player, GameObject player2)
        {
            shootLabel.SetActive(false);

            yield return new WaitForSeconds(3f);

            winnerLabel.gameObject.SetActive(false);
            
            // player.GetComponent<PlayerShootOnline>().RpcSubscribeToShootingEvent();
            // player2.GetComponent<PlayerShootOnline>().RpcSubscribeToShootingEvent();
            // player.GetComponent<Animator>().SetTrigger("RoundFinish");
            // player2.GetComponent<Animator>().SetTrigger("RoundFinish");
            player.GetComponent<NetworkAnimator>().SetTrigger("RoundFinish");
            player2.GetComponent<NetworkAnimator>().SetTrigger("RoundFinish");
        }

        [ClientRpc]
        private void RpcRestartRoundOnClients(GameObject player, GameObject player2)
        {
            StartCoroutine(FinishRoundOnClients(player, player2));
        }

        private void CheckEndGame()
        {
            int player1Score = scoreController.GetP1Score();
            int player2Score = scoreController.GetP2Score();

            if (player1Score < 2 && player2Score < 2)
            {
                FinishRound();
            } 
            else if (player1Score == 2 || player2Score == 2)
            {
                OnGameEnd?.Invoke();
                shootLabel.SetActive(false);
                RpcDisableShootLabel();
            }
        }

        [ClientRpc]
        private void RpcDisableShootLabel()
        {
            shootLabel.SetActive(false);
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
