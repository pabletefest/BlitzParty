using Mirror;
using UnityEngine;

namespace Online
{
    public class PlayerIndicatorUI : NetworkBehaviour
    {
        [SerializeField] private GameObject arrowP1;
        [SerializeField] private GameObject arrowP2;

        [TargetRpc]
        public void StartAnimationIndicator(NetworkConnection target, int playerNumber)
        {
            if (playerNumber == 1)
            {
                arrowP1.SetActive(true);
            }
            else if (playerNumber == 2)
            {
                arrowP2.SetActive(true);
            }
        }
    }
}
