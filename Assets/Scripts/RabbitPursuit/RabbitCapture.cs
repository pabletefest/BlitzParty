using System;
using UnityEngine;

namespace RabbitPursuit
{
    public class RabbitCapture : MonoBehaviour
    {

        public static event Action<GameObject> OnEnemyCaptured;
        PlayersScore scoreController;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Rabbit")
            {
                //Destroy(collision.gameObject);
                //ServiceLocator.Instance.GetService<>
            
                collision.gameObject.SetActive(false);
                scoreController.P2ScorePoints(1);
                OnEnemyCaptured?.Invoke(collision.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
        }
    }
}
