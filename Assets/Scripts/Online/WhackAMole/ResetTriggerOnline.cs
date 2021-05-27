using UnityEngine;

namespace Online.WhackAMole
{
    public class ResetTriggerOnline : StateMachineBehaviour
    {
        //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.gameObject.CompareTag("Mole"))
            {
                animator.ResetTrigger("MoleHit");
            }
            else if (animator.gameObject.CompareTag("GoldMole"))
            {
                animator.ResetTrigger("GoldMoleHit");
            }
            else if (animator.gameObject.CompareTag("ZoomyWhackAMole"))
            {
                animator.ResetTrigger("ZoomyHit");
            }
        }

    }
}

