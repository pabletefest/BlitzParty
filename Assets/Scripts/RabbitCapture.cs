using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCapture : MonoBehaviour
{

    PlayersScore scoreController;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Rabbit")
        {
            Destroy(collision.gameObject);
            scoreController.P2ScorePoint();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
