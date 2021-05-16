using System;
using CowboyDuel;
using UnityEngine;

public class EnemyShoot : MonoBehaviour, IShootable
{
    public event Action<string, float> OnShot;
        
    [SerializeField] private CountdownUI countdownUI;
    [SerializeField] private Animator enemyAnimator;
    
    private bool canShoot;
    
    private float shootTime;
    private float remainingTime;
    
    private void OnEnable()
    {
        countdownUI.OnCountdownOver += ShootingTime;
    }


    private void OnDisable()
    {
        countdownUI.OnCountdownOver -= ShootingTime;
    }
    
    private void ShootingTime()
    {
        canShoot = true;
        shootTime = UnityEngine.Random.Range(0.2f, 0.4f);
        remainingTime = shootTime;
        Debug.Log("Enemy can shoot now");
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
