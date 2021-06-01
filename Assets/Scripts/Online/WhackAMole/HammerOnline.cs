using Services;
using UnityEngine;
using Mirror;

namespace Online.WhackAMole
{
    public class HammerOnline : NetworkBehaviour
    {
        private PlayersScoreOnline scoreController;
        private HammerSpawnerOnline playerOwner;

        public override void OnStartClient()
        {
            Debug.Log("Client started");
            scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScoreOnline>();
            Debug.Log(scoreController);
        }

        [ClientRpc]
        public void SetPlayerOwner(GameObject player)
        {
            playerOwner = player.GetComponent<HammerSpawnerOnline>();
            Debug.Log(playerOwner);
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("A fucking mole was hit madafaka");
            Debug.Log(scoreController);
            Debug.Log(playerOwner);

            Animator animator = other.gameObject.GetComponent<Animator>();

            if (other.CompareTag("Mole"))
            {
                //SpawnSoundEffect(other.tag);
                animator.SetTrigger("MoleHit");
                scoreController.PlayerScorePoints(1, playerOwner.PlayerNumber);
            }
            else if (other.CompareTag("GoldMole"))
            {
                //SpawnSoundEffect(other.tag);
                animator.SetTrigger("GoldMoleHit");
                scoreController.PlayerScorePoints(5, playerOwner.PlayerNumber);
            }
            else if (other.CompareTag("ZoomyWhackAMole"))
            {
                //SpawnSoundEffect(other.tag);
                animator.SetTrigger("ZoomyHit");
                scoreController.PlayerScorePoints(-3, playerOwner.PlayerNumber);
            }

            if (other.CompareTag("Background")) return;

            other.enabled = false;
        }

        [TargetRpc]
        private void SpawnSoundEffect(string tag) 
        {
            if (tag == "Mole")
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitMole");
            }
            else if (tag == "GoldMole")
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitGoldenMole");
            }
            else if (tag == "ZoomyWhackAMole")
            {
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HitZoomy");
            }
        }
    }
}
