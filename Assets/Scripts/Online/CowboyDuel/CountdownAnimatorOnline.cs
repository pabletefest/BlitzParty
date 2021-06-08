using System;
using Mirror;

namespace Online.CowboyDuel
{
    public class CountdownAnimatorOnline : NetworkBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}