using System;
using CowboyDuel;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class PlayerShootOnline : MonoBehaviour, IShootable
    {
        public event Action<string, float> OnShot;
        
        [SerializeField] private CountdownUIOnline countdownUI;
        [SerializeField] private Animator playerAnimator;
        
        private bool canShoot;
        private bool hasShotEarly;
        private bool hasShootAppeared;

        private float timeSinceReady;

        private Camera mainCamera;
        
        private float limitShootTime = 1.2f;

        private void OnEnable()
        {
            countdownUI.OnCountdownOver += ShootingTime;
            countdownUI.OnShootAppeared += EnableCorrectShoot;
        }

        private void OnDisable()
        {
            countdownUI.OnCountdownOver -= ShootingTime;
            countdownUI.OnShootAppeared -= EnableCorrectShoot;
        }

        void Awake()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
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
            
            OnShot?.Invoke(gameObject.tag, timeSinceReady);
            
            // Debug.Log($"hasShootAppeared: {hasShootAppeared}");
            canShoot = false;
            limitShootTime = 1.2f;
            timeSinceReady = 0;
            hasShootAppeared = false;
            
            Debug.Log("Player shot");
        }
    
        private void ShootingTime()
        {
            canShoot = true;
            Debug.Log("Player can shoot now");
        }
        
        private void EnableCorrectShoot(bool shootAppeared)
        {
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
    }
}
