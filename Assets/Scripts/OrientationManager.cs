using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public void ChangeScreenPortrait(bool isPortrait)
    {
        if (isPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

}
