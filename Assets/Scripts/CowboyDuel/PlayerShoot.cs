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
        private bool playerClicked;

        private float timeSinceReady;

        private void OnEnable()
        {
            countdownUI.OnCountdownOver += ShootingTime;
        }


        private void OnDisable()
        {
            countdownUI.OnCountdownOver -= ShootingTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (canShoot)
            {
                timeSinceReady += Time.deltaTime;
                
                #if UNITY_EDITOR

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
            }
        }

        public void Shoot()
        {
            playerAnimator.SetTrigger("Shoot");
            canShoot = false;
            playerClicked = false;
            OnShot?.Invoke(gameObject.tag, timeSinceReady);
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

        private void OnMouseDown()
        {
            playerClicked = true;
            Debug.Log("Player clicked the screen");
        }
    }
}
