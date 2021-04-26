using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = other.gameObject.GetComponent<Animator>();

        if (other.CompareTag("Mole"))
        {
            animator.SetTrigger("MoleHit");
        }
        else if (other.CompareTag("GoldMole"))
        {
            animator.SetTrigger("GoldMoleHit");
        }
        else if (other.CompareTag("ZoomyWhackAMole"))
        {
            animator.SetTrigger("ZoomyHit");
        }
    }
}