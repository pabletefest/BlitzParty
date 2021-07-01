using UnityEngine;
using Services;

namespace CowboyDuel
{
    public class GameInitializer : MonoBehaviour
    {
        
        void Start()
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("CowboyDuelTheme");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
