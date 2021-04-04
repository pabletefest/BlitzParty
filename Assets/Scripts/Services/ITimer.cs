using System;
public interface ITimer
{
    event Action OnTimerOver;
    void SetTimeInSeconds(float timeInSeconds);
    float GetCurrentTime();
    void StartTimer();
    void StopTimer();
    void ResetTimer();
}
