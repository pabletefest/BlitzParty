using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CowboyDuel
{
    public class CountdownUI : MonoBehaviour
    {
        public event Action OnCountdownOver;
    
        [SerializeField] private float startingTime = 3f;

        private float time;

        [SerializeField] private Text countdownText;
        [SerializeField] private GameObject shootLabel;
    
        private void Awake()
        {
            time = startingTime;
        }

        public IEnumerator StartCountdown()
        {

            countdownText.gameObject.SetActive(true);
        
            countdownText.text = Mathf.Ceil(time).ToString();

            while (time > 0f)
            {
                countdownText.text = Mathf.Ceil(time).ToString();
                //countdownText.text = ((int) time).ToString();
                //Debug.Log($"CurrentTime Timer: {currentTime}");
                time -= Time.deltaTime;
                yield return null;
            }

            float randomShootTime = UnityEngine.Random.Range(0.5f, 4f);
        
            while (randomShootTime > 0f)
            {
                randomShootTime -= Time.deltaTime;
                yield return null;
            }
        
            shootLabel.SetActive(true);

            if (time <= 0f)
            {
                time = 0f;
                OnCountdownOver?.Invoke();
            }

            countdownText.gameObject.SetActive(false);

            time = startingTime;
        }
    }
}
