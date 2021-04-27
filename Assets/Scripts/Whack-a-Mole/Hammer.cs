using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    PlayersScore scoreController;
    void Start()
    {
        scoreController = GameObject.Find("ScoreController").GetComponent<PlayersScore>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = other.gameObject.GetComponent<Animator>();

        if (other.CompareTag("Mole"))
        {
            animator.SetTrigger("MoleHit");
            scoreController.P1ScorePoints(1);
        }
        else if (other.CompareTag("GoldMole"))
        {
            animator.SetTrigger("GoldMoleHit");
            scoreController.P1ScorePoints(5);
        }
        else if (other.CompareTag("ZoomyWhackAMole"))
        {
            animator.SetTrigger("ZoomyHit");
            scoreController.P1SubstractPoints(3);
        }
    }
}