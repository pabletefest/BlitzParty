using UnityEngine;

namespace Online.WhackAMole
{
    public class DeactivateOnExitOnline : StateMachineBehaviour
    {
        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Im exiting running state");
            Destroy(animator.gameObject);
        }
    }
}
