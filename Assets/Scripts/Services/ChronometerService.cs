using System;
using System.Collections;
using UnityEngine;
public class ChronometerService : ITimer 
{
    MonoBehaviour monoBehaviour;
    IEnumerator coroutine;
    private float time;
    private float currentTime;

    public event Action OnTimerOver;

    public ChronometerService()
    {
        SetTimeInSeconds(0f);
    }

    public void SetMonobehaviour(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour;
    }

    public void SetTimeInSeconds(float timeInSeconds)
    {
        time = timeInSeconds;
    }

    public float GetCurrentTime() => currentTime;

    public void StartTimer()
    {
        coroutine = StartCountdown();
        monoBehaviour.StartCoroutine(coroutine);
    }

    public void StopTimer()
    {
        monoBehaviour.StopCoroutine(coroutine);
    }

    public void ResetTimer()
    {
        currentTime = time;
    }

    private IEnumerator StartCountdown()
    {
        currentTime = time;

        while (currentTime > 0f)
        {
            //Debug.Log($"CurrentTime Timer: {currentTime}");
            currentTime -= Time.deltaTime;
            yield return null;
        }

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            OnTimerOver?.Invoke();
        }
    }

}
