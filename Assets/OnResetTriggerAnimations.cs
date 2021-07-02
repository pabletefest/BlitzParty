using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnResetTriggerAnimations : StateMachineBehaviour
{
    [SerializeField] private NetworkAnimator networkAnimator;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        networkAnimator.ResetTrigger("Shoot");
        networkAnimator.ResetTrigger("MissShot");
        networkAnimator.ResetTrigger("Death");
        networkAnimator.ResetTrigger("RoundFinish");
    }
}

