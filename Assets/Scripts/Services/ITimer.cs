using System;

namespace Services
{
    public interface ITimer
    {
        event Action OnTimerOver;
        void SetTimeInSeconds(float timeInSeconds);
        float GetCurrentTime();
        float GetTotalTime();
        void Tick(float deltaTime);
        void CheckForTimerEnd();
        void StartTimerMonobehaviour();
        void StopTimerMonobehaviour();
        void ResetTimer();
        void RestartTimerMonobehaviour();
    }
}
