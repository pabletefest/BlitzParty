using UnityEngine;
public class ChronometerService :ITimer 
{
    MonoBehaviour monoBehaviour;
    private float time; 

    public ChronometerService()
    {
        time = 0;
    }

    public void SetMonobehaviour(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
    }

    public void SetTimeInSeconds(float timeInSeconds)
    {
        time = timeInSeconds;
    }

    public void StartTimer()
    {
        throw new System.NotImplementedException();
    }

    public void StopTimer()
    {
        throw new System.NotImplementedException();
    }

    public void ResetTimer()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator StartCountdown()
    {
        
    }
}
