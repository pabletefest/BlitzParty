using UnityEngine;
using Services;

namespace CowboyDuel
{
    public class GameInitializer : MonoBehaviour
    {
        
        void Start()
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("CowboyDuelTheme");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
