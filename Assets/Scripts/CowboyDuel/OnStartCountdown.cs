using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartCountdown : StateMachineBehaviour
{
    private CountdownUI gameCountdown;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gameCountdown = GameObject.FindGameObjectWithTag("CountdownUI").GetComponent<CountdownUI>();
        gameCountdown.StartCoroutine(gameCountdown.StartCountdown());
    }
}
