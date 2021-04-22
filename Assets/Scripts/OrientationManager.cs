using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    [SerializeField]
    public bool isPortrait;


    void Start()
    {
      
        if(isPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

    }

}
