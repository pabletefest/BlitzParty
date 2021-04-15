using System;
using System.Collections;
using UnityEngine;

namespace Services
{
    public class ChronometerService : ITimer 
    {
        /*
        [SerializeField]
        private TimeChronometerSO timeChronometerSO;
        */
        MonoBehaviour monoBehaviour;
        IEnumerator coroutine;
        private float time;
        private float currentTime;

        public event Action OnTimerOver;

        public ChronometerService(TimeChronometerSO timeChronometerSO)
        {
            //this.timeChronometerSO = timeChronometerSO;
            SetTimeInSeconds(timeChronometerSO.TimeInSeconds); //Initial time set on ScriptableObject
        }

        public void SetMonobehaviour(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }

        public void SetTimeInSeconds(float timeInSeconds)
        {
            time = timeInSeconds;
            currentTime = timeInSeconds;
        }

        public float GetCurrentTime() => currentTime;
        public float GetTotalTime() => time;

        public void StartTimerMonobehaviour()
        {
            coroutine = StartCountdownCoroutine();
            monoBehaviour.StartCoroutine(coroutine);
        }

        public void StopTimerMonobehaviour()
        {
            monoBehaviour.StopCoroutine(coroutine);
        }

        public void ResetTimer()
        {
            currentTime = time;
        }

        public void RestartTimerMonobehaviour()
        {
            ResetTimer();
            StartTimerMonobehaviour();
        }


        public void Tick(float deltaTime)
        {
            if (currentTime <= 0) return;

            currentTime -= deltaTime;

            CheckForTimerEnd();
        }

        public void CheckForTimerEnd()
        {
            if (currentTime > 0) return;
            
            currentTime = 0;

            OnTimerOver?.Invoke();
        }

        private IEnumerator StartCountdownCoroutine()
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
}
