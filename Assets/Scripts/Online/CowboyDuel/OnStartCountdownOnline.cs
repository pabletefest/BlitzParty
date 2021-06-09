using Mirror;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class OnStartCountdownOnline : StateMachineBehaviour
    {
        private CountdownUIOnline gameCountdown;
        // private static int counter;
    
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Allow to start the countdown");

            gameCountdown = GameObject.Find("GUIController").GetComponent<CountdownUIOnline>();
            gameCountdown.EnableCountdownStart();

            /*Debug.Log($"Counter: {counter}");
            if (counter == 0)
            {
                gameCountdown = GameObject.Find("GUIController").GetComponent<CountdownUIOnline>();
                Debug.Log("Starting the countdown");
                gameCountdown.CmdStartCountdown();
                counter++;
            }*/
            // int playerNumber = animator.GetComponent<NetworkIdentity>(9;
            // gameCountdown = GameObject.Find("GUIController").GetComponent<CountdownUIOnline>();
            // gameCountdown.CmdStartCountdown();
        }
    }
}
