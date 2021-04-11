using System;
public interface ITimer
{
    event Action OnTimerOver;
    void SetTimeInSeconds(float timeInSeconds);
    float GetCurrentTime();
    float GetTotalTime();
    void StartTimer();
    void StopTimer();
    void ResetTimer();
    void RestartTimer();
}
