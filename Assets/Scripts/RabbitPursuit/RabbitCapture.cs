using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;

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
            scoreController.P2ScorePoint();
            OnEnemyCaptured?.Invoke(collision.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
    }
}
