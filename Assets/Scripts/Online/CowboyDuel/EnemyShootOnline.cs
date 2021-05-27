using System;
using CowboyDuel;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class EnemyShootOnline : MonoBehaviour, IShootable
    {
        public event Action<string, float> OnShot;
        
        [SerializeField] private CountdownUIOnline countdownUI;
        [SerializeField] private Animator enemyAnimator;
    
        private bool canShoot;
    
        private float shootTime;
        private float remainingTime;
    
        private void OnEnable()
        {
            countdownUI.OnShootAppeared += ShootingTime;
        }
        
        private void OnDisable()
        {
            countdownUI.OnShootAppeared -= ShootingTime;
        }
    
        private void ShootingTime(bool hasShootAppeared)
        {
            if (hasShootAppeared)
            {
                canShoot = true;
                shootTime = UnityEngine.Random.Range(0.2f, 0.75f);
                remainingTime = shootTime;
                Debug.Log("Enemy can shoot now");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (canShoot)
            {
                if (remainingTime > 0)
                {
                    remainingTime -= Time.deltaTime;
                }
                else
                {
                    Shoot();
                }
            }
        }

        public void Shoot()
        {
            enemyAnimator.SetTrigger("Shoot");
            canShoot = false;
            OnShot?.Invoke(gameObject.tag, shootTime);
            Debug.Log("Enemy shot");
        }
    }
}
