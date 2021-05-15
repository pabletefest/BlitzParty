using UnityEngine;
using Services;

namespace WhackAMole
{
    public class Hammer : MonoBehaviour
    {
        private PlayersScore scoreController;
        void Awake()
        {
            scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            Animator animator = other.gameObject.GetComponent<Animator>();

            if (other.CompareTag("Mole"))
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitMole");
                animator.SetTrigger("MoleHit");
                scoreController.P1ScorePoints(1);
            }
            else if (other.CompareTag("GoldMole"))
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitGoldenMole");
                animator.SetTrigger("GoldMoleHit");
                scoreController.P1ScorePoints(5);
            }
            else if (other.CompareTag("ZoomyWhackAMole"))
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitZoomy");
                animator.SetTrigger("ZoomyHit");
                scoreController.P1SubstractPoints(3);
            }

            other.enabled = false;
        }
    }
}
