using System;
using UnityEngine;

namespace CowboyDuel
{
    public class PlayerShoot : MonoBehaviour, IShootable
    {
        public event Action<string, float> OnShot;
        
        [SerializeField] private CountdownUI countdownUI;
        [SerializeField] private Animator playerAnimator;
        
        private bool canShoot;

        private float timeSinceReady;

        private Camera mainCamera;
        
        private float limitShootTime = 1.2f;

        private void OnEnable()
        {
            countdownUI.OnCountdownOver += ShootingTime;
        }


        private void OnDisable()
        {
            countdownUI.OnCountdownOver -= ShootingTime;
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
                
                timeSinceReady += Time.deltaTime;
                
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

        public void Shoot()
        {
            playerAnimator.SetTrigger("Shoot");
            canShoot = false;
            limitShootTime = 1.2f;
            OnShot?.Invoke(gameObject.tag, timeSinceReady);
            timeSinceReady = 0;
            Debug.Log("Player shot");
            
        }
    
        private void ShootingTime()
        {
            canShoot = true;
            Debug.Log("Player can shoot now");
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
