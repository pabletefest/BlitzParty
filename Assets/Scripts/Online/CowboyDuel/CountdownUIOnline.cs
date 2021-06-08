using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Online.CowboyDuel
{
    public class CountdownUIOnline : NetworkBehaviour
    {
        public event Action OnCountdownOver;
        public event Action<bool> OnShootAppeared;
    
        [SerializeField] private float startingTime = 3f;

        private float time;

        [SerializeField] private Text countdownText;
        [SerializeField] private GameObject shootLabel;
        
        [SyncVar(hook = nameof(OnTimeChanged))]
        private float serverTime;
    
        private void Awake()
        {
            time = startingTime;
        }
        
        private void OnTimeChanged(float oldValue, float newValue)
        {
            UpdateCountdownOnline(newValue);
        }

        private void UpdateCountdownOnline(float newValue)
        {
            countdownText.text = Mathf.Ceil(newValue).ToString();
        }

        public IEnumerator StartCountdown()
        {
            if (!isServer) yield break;
            
            OnShootAppeared?.Invoke(false);
            
            countdownText.gameObject.SetActive(true);
            RpcShowCountdownOnClients();
        
            countdownText.text = Mathf.Ceil(time).ToString();
            serverTime = time;
            
            while (time > 0f)
            {
                Debug.Log($"Countdown time: {time}");
                countdownText.text = Mathf.Ceil(time).ToString();
                serverTime = time;
                //countdownText.text = ((int) time).ToString();
                //Debug.Log($"CurrentTime Timer: {currentTime}");
                time -= Time.fixedDeltaTime;
                //serverTime = time;
                yield return null;
            }

            if (time <= 0f)
            {
                time = 0f;
                serverTime = time;
                OnCountdownOver?.Invoke();
            }
            
            float randomShootTime = UnityEngine.Random.Range(0.5f, 4f);
            //Debug.Log($"Random time to shoot: {randomShootTime}");
        
            while (randomShootTime > 0f)
            {
                //Debug.Log($"Random time to shoot: {randomShootTime}");
                randomShootTime -= Time.deltaTime;
                yield return null;
            }
        
            shootLabel.SetActive(true);
            RpcShowShootLabelOnClients();
            
            OnShootAppeared?.Invoke(true);
            
            countdownText.gameObject.SetActive(false);
            RpcHideCountdownOnClients();

            time = startingTime;
        }

        [Command(requiresAuthority =  false)]
        public void CmdStartCountdown()
        {
            StartCoroutine(StartCountdown());
        }
        
        [ClientRpc]
        private void RpcShowCountdownOnClients()
        {
            countdownText.gameObject.SetActive(true);
        }
        
        [ClientRpc]
        private void RpcShowShootLabelOnClients()
        {
            shootLabel.SetActive(true);
        }
        
        [ClientRpc]
        private void RpcHideCountdownOnClients()
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}
