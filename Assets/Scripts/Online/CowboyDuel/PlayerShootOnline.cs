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
        [SerializeField] private WinnerCheckerOnline winnerChecker;
        
        [SyncVar]
        private bool canShoot;
        
        private bool hasShotEarly;
        
        [SyncVar]
        private bool hasShootAppeared;

        private float timeSinceReady;

        private Camera mainCamera;
        
        private float limitShootTime = 1.2f;
        
        [SyncVar]
        public int playerNumber;

        public int PlayerNumber => playerNumber;

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
            mainCamera = Camera.main;
            countdownUI = GameObject.Find("GUIController").GetComponent<CountdownUIOnline>();
            countdownUI.OnCountdownOver += ShootingTime;
            countdownUI.OnShootAppeared += EnableCorrectShoot;
            winnerChecker = GameObject.Find("WinnerController").GetComponent<WinnerCheckerOnline>();
        }
        

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer) return;
            
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
                        Debug.Log("Auto shoot");
                        Shoot();
                    }
                }
            }
        }

        public void Shoot()
        {
            if (!hasShootAppeared)
            {
                hasShotEarly = true;
            }
            
            //Debug.Log($"hasShotEarly: {hasShotEarly}");
                
            if (hasShotEarly)
            {
                // Debug.Log("Shot Miss");
                playerAnimator.SetTrigger("ShotMiss");
                timeSinceReady = 2f;
                hasShotEarly = false;
            }
            else
            {
                // Debug.Log("Correct Shot");
                playerAnimator.SetTrigger("Shoot");
            }
            
            // Debug.Log($"timeSinceReady: {timeSinceReady}");
            // Debug.Log($"limitShootTime: {limitShootTime}");
            
            //OnShot?.Invoke(PlayerNumber, timeSinceReady);
            Debug.Log($"Player {PlayerNumber} sending data to server, shoot time: {timeSinceReady}");
            CmdPassShotDataToServer();
            
            // Debug.Log($"hasShootAppeared: {hasShootAppeared}");
            //canShoot = false;
            CmdDisablePlayersShoot();
            limitShootTime = 1.2f;
            timeSinceReady = 0;
            //hasShootAppeared = false;
            
            Debug.Log("Player shot");
        }

        [Command]
        private void CmdPassShotDataToServer(NetworkConnectionToClient  sender = null)
        {
            //OnShot?.Invoke(PlayerNumber, timeSinceReady);
            winnerChecker.RecieveClientsData(timeSinceReady, sender);
        }

        [Command]
        private void CmdDisablePlayersShoot()
        {
            canShoot = false;
            hasShootAppeared = false;
        }
    
        private void ShootingTime()
        {
            if (!isServer) return;
            
            canShoot = true;
            Debug.Log("Player can shoot now");
        }
        
        private void EnableCorrectShoot(bool shootAppeared)
        {
            if (!isServer) return;
            
            hasShootAppeared = shootAppeared;
        }

        private bool CheckPlayerTouch()
        {
            if (Input.touches.Length > 0)
            {
                return true;
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
    }
}
