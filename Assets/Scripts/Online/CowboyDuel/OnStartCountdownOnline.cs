using UnityEngine;

namespace Online.CowboyDuel
{
    public class OnStartCountdownOnline : StateMachineBehaviour
    {
        private CountdownUIOnline gameCountdown;
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            gameCountdown = GameObject.FindGameObjectWithTag("CountdownUIOnline").GetComponent<CountdownUIOnline>();
            gameCountdown.StartCoroutine(gameCountdown.StartCountdown());
        }
    }
}
