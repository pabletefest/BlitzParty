using System.Collections;
using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    public void ChangeScreenPortrait(bool isPortrait)
    {
        StartCoroutine(StartOrientationChange(isPortrait));
    }

    private IEnumerator StartOrientationChange(bool isPortrait)
    {
        if (isPortrait)
        {
            bool completed = Screen.orientation == ScreenOrientation.Portrait;
            Screen.orientation = ScreenOrientation.Portrait;
            while (!completed)
            {
                completed = (Screen.orientation == ScreenOrientation.Portrait);
                yield return null;
            }
        }
        else
        {
            bool completed = Screen.orientation == ScreenOrientation.Landscape;
            Screen.orientation = ScreenOrientation.Landscape;
            while (!completed)
            {
                completed = (Screen.orientation == ScreenOrientation.Landscape);
                yield return null;
            }
        }
        
    }

}
