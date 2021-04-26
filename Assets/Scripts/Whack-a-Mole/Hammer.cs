using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    private float hitTime;
    private float hitRate = 0.5f;
    private bool hitPossible;

    private void OnMouseDown()
    {
        if (hitPossible)
        {
            
            hitPossible = false;
        }
    }

    void Awake()
    {
        hitTime = hitRate;
        hitPossible = true;
    }

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

    // Update is called once per frame
    void Update()
    {
        if (hitTime <= 0)
        {
            hitPossible = true;
            hitTime = hitRate;
        }
        else
        {
            hitTime -= Time.deltaTime;
        }
    }
}