using Services;
using UnityEngine;

namespace Online.CowboyDuel
{
    public class GameInitializerOnline : MonoBehaviour
    {
        void Start()
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlayMinigameTheme("CowboyDuelTheme");
        }
    }
}
