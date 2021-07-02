using System;
using CowboyDuel;
using Mirror;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class PlayerShootOnline : NetworkBehaviour, IShootable
    {
        public event Action<int, float> OnShot;
        
        [SerializeField] private CountdownUIOnline countdownUI;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private NetworkAnimator networkAnimator;
        [SerializeField] private WinnerCheckerOnline winnerChecker;
        
        // [SyncVar]
        [SerializeField]private bool canShoot;
        
        // [SyncVar]
        private bool hasShotEarly;
        
        // [SyncVar]
        private bool hasShootAppeared;

        // [SyncVar]
        private float timeSinceReady;

        private Camera mainCamera;
        
        private float limitShootTime = 1.2f;
        
        [SyncVar]
        public int playerNumber;

        public int PlayerNumber => playerNumber;
        
        
        // [SyncVar(hook = nameof(OnPlayerDead))] 
        // public bool isDead;
        //
        // [SyncVar(hook = nameof(OnRoundOver))] 
        // public bool isRoundOver;

        // private int numberOfCalls;
        
        /*private void OnEnable()
        {
            countdownUI.OnCountdownOver += ShootingTime;
            countdownUI.OnShootAppeared += EnableCorrectShoot;
        }

        private void OnDisable()
        {
            countdownUI.OnCountdownOver -= ShootingTime;
            countdownUI.OnShootAppeared -= EnableCorrectShoot;
        }*/

        public override void OnStartClient()
        {
            Debug.Log("OnStartClient getting called!!");
            Debug.Log($"I have authority {hasAuthority} and I'm player {PlayerNumber}");
            mainCamera = Camera.main;

            networkAnimator = GetComponent<NetworkAnimator>();
            
            winnerChecker = GameObject.Find("WinnerController").GetComponent<WinnerCheckerOnline>();
            Debug.Log($"OnStartClient called, is winnerChecker null? {winnerChecker}");
            winnerChecker.SetPlayer(this, PlayerNumber);
            
        }

        public override void OnStartServer()
        {
            Debug.Log("OnStartServer getting called!!");
            countdownUI = GameObject.Find("GUIController").GetComponent<CountdownUIOnline>();
            countdownUI.OnCountdownOver += ShootingTime;
            countdownUI.OnShootAppeared += EnableCorrectShoot;
            
            winnerChecker = GameObject.Find("WinnerController").GetComponent<WinnerCheckerOnline>();
            Debug.Log($"OnStartServer called, is winnerChecker null? {winnerChecker}");
            //winnerChecker.SetPlayer(this, PlayerNumber);
        }

        /*private void OnPlayerDead(bool oldValue, bool newValue)
        {
            Debug.Log("OnPlayerDead called!!");
            Debug.Log($"Checking value isDead before: {isDead} in player {PlayerNumber}");
            // if (newValue & hasAuthority)
            if (newValue)
            {
                networkAnimator.SetTrigger("Death");
                isDead = false;
            }
            Debug.Log($"Checking value isDead after: {isDead} in player {PlayerNumber}");
        }
        
        private void OnRoundOver(bool oldValue, bool newValue)
        {
            numberOfCalls++;
            Debug.Log(numberOfCalls);
            Debug.Log("OnRoundOver called!!");
            Debug.Log($"Checking value isRoundOver before: {isRoundOver} in player {PlayerNumber}");
            // if (newValue & hasAuthority)
            if (newValue)
            {
                networkAnimator.SetTrigger("RoundFinish");
                isRoundOver = false;
            }
            Debug.Log($"Checking value isRoundOver after: {isRoundOver} in player {PlayerNumber}");
        }*/


        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) return;
            //Debug.Log($"Can I shoot? {canShoot}");
                
            if (canShoot)
            {
                #if UNITY_EDITOR

                    bool playerClicked = CheckPlayerClick();
                    
                    if (playerClicked)
                    {
                        Shoot();
                    }

                #else

                    bool isMobileDeviceTouch = CheckPlayerTouch();
                
                    if (isMobileDeviceTouch)
                    {
                        Shoot();
                    }

                #endif

                if (hasShootAppeared)
                {
                    timeSinceReady += Time.deltaTime;
                    
                    if(limitShootTime > 0)
                    {
                        limitShootTime -= Time.deltaTime;
                    } 
                    else
                    {
                        Debug.Log("Auto shoot " + PlayerNumber);
                        Shoot();
                        limitShootTime = 1.2f;
                        canShoot = false;
                        hasShootAppeared = false;
                    }
                }
            }
        }
        
        public void Shoot()
        {
            if (!hasShootAppeared)
            {
                // hasShotEarly = true;
                CmdTouchedBeforeLabel();
            }
            
            //Debug.Log($"hasShotEarly: {hasShotEarly}");
                
            if (hasShotEarly)
            {
                // Debug.Log("Shot Miss");
                networkAnimator.SetTrigger("ShotMiss");
                //CmdAnimationShotMiss();
                CmdShotEarly();
                // timeSinceReady = 2f;
                // hasShotEarly = false;
            }
            else
            {
                // Debug.Log("Correct Shot");
                //CmdAnimationShoot();
                networkAnimator.SetTrigger("Shoot");
                canShoot = false;
                hasShootAppeared = false;
                
            }
            
            // Debug.Log($"timeSinceReady: {timeSinceReady}");
            // Debug.Log($"limitShootTime: {limitShootTime}");
            
            //OnShot?.Invoke(PlayerNumber, timeSinceReady);
            Debug.Log($"Player {PlayerNumber} sending data to server, shoot time: {timeSinceReady}");
            CmdPassShotDataToServer(timeSinceReady);
            
            // Debug.Log($"hasShootAppeared: {hasShootAppeared}");
            //canShoot = false;
            //CmdDisablePlayersShoot();
            
            /*canShoot = false;
            hasShootAppeared = false;
            limitShootTime = 1.2f;
            timeSinceReady = 0;*/
            
            //hasShootAppeared = false;
            
            Debug.Log("Player shot");
        }

        [Command]
        private void CmdPassShotDataToServer(float time, NetworkConnectionToClient  sender = null)
        {
            RpcRestartShootData();
            //OnShot?.Invoke(PlayerNumber, timeSinceReady);
            uint playerNetId = sender.identity.netId;

            if (playerNetId == 4)
            {
                winnerChecker.RecieveClient1Data(time);
            }
            else if (playerNetId == 5)
            {
                winnerChecker.RecieveClient2Data(time);
            }

        }

        [ClientRpc]
        private void RpcRestartShootData()
        {
            canShoot = false;
            hasShootAppeared = false;
            limitShootTime = 1.2f;
            timeSinceReady = 0;
            //networkAnimator.ResetTrigger("Shoot");
        }
        
        [Command]
        private void CmdTouchedBeforeLabel()
        {
            RpcTouchedBeforeLabel();
            // hasShotEarly = true;
        }
        
        [ClientRpc]
        private void RpcTouchedBeforeLabel()
        {
            hasShotEarly = true;
        }

        [Command]
        private void CmdAnimationShotMiss()
        {
            networkAnimator.SetTrigger("ShotMiss");
        }
        
        [Command]
        private void CmdAnimationShoot()
        {
            networkAnimator.SetTrigger("Shoot");
            Debug.Log("CanShoot at shooting moment is " + canShoot);
            Debug.Log("LimitShootTime at shooting moment is " + limitShootTime);
            Debug.Log("HasShootAppeared at shooting moment is " + hasShootAppeared);
            
        }
        
        [Command]
        private void CmdShotEarly()
        {
            // timeSinceReady = 2f;
            // hasShotEarly = false;
            RpcShotEarly();
        }
        
        [ClientRpc]
        private void RpcShotEarly()
        {
            timeSinceReady = 2f;
            hasShotEarly = false;
            //networkAnimator.ResetTrigger("ShotMiss");
        }

        /*[Command]
        private void CmdDisablePlayersShoot()
        {
            canShoot = false;
            hasShootAppeared = false;
        }*/
    
        public void ShootingTime()
        {
            if (!isServer) return;

            var playerConns = ((CowboyDuelNetworkManager) NetworkManager.singleton).PlayersConnections;
            
            foreach (var playerConn in playerConns)
            {
                TargetAllowPlayerToShot(playerConn.Value);
            }
            
            // canShoot = true;
            //countdownUI.OnCountdownOver -= ShootingTime;
            //RpcAllowPlayersToShot();
            Debug.Log($"Player {PlayerNumber} can shoot now");
            Debug.Log($"Limit shoot for Player {PlayerNumber} time at shoot appear is " + limitShootTime);
        }

        [TargetRpc]
        private void TargetAllowPlayerToShot(NetworkConnection target)
        {
            canShoot = true;
        }
        
        private void EnableCorrectShoot(bool shootAppeared)
        {
            if (!isServer) return;
            
            var playerConns = ((CowboyDuelNetworkManager) NetworkManager.singleton).PlayersConnections;
            
            foreach (var playerConn in playerConns)
            {
                TargetEnableAutoShoot(playerConn.Value, shootAppeared);
            }
            
            //hasShootAppeared = shootAppeared;
        }
        
        [TargetRpc]
        private void TargetEnableAutoShoot(NetworkConnection target, bool shootAppeared)
        {
            hasShootAppeared = shootAppeared;
        }


        private bool CheckPlayerTouch()
        {
            if (Input.touches.Length > 0)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                
                if (hit.collider.CompareTag("Background"))
                {
                    return true;
                    //Debug.Log("Player clicked the screen");
                }
                //return true;
            }

            return false;
        }

        private bool CheckPlayerClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                
                if (hit.collider.CompareTag("Background"))
                {
                    return true;
                    //Debug.Log("Player clicked the screen");
                }
            }

            return false;
        }

        [ClientRpc]
        public void RpcEnablePlayerAnimator()
        {
            playerAnimator.enabled = true;
        }
        
        [ClientRpc]
        public void RpcSubscribeToShootingEvent()
        {
            countdownUI.OnCountdownOver += ShootingTime;
        }

        [ClientRpc]
        public void RpcSetDeathAnimation()
        {
            if (!isLocalPlayer) return;
            
            //CmdAnimationDeath();
             networkAnimator.SetTrigger("Death");
        }

        [Command]
        private void CmdAnimationDeath()
        {
            networkAnimator.SetTrigger("Death");
        }
        
        [ClientRpc]
        public void RpcSetRoundFinishAnimation()
        {
            if (!isLocalPlayer) return;

            //CmdAnimationRoundFinish();
             networkAnimator.SetTrigger("RoundFinish");
        }
        
        [Command]
        private void CmdAnimationRoundFinish()
        {
            networkAnimator.SetTrigger("RoundFinish");
        }
        
        private void OnDestroy()
        {
            NetworkClient.Shutdown();
        }
    }
}
