using Mirror;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class OnResetTriggerAnimations : StateMachineBehaviour
    {
        private NetworkAnimator networkAnimator;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            networkAnimator = animator.GetComponent<NetworkAnimator>();
            
            networkAnimator.ResetTrigger("Shoot");
            networkAnimator.ResetTrigger("MissShot");
            networkAnimator.ResetTrigger("Death");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            networkAnimator.ResetTrigger("RoundFinish");
        }
    }
}

