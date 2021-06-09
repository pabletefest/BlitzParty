using System;
using System.Collections;
using Mirror;
using Online.BinkyPursuit;
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

        [SyncVar] private float serverRandomTime;

        private bool isAbleToStart;

        [SerializeField] private PlayerShootOnline player;

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

        private void Update()
        {
            ObtainPlayerReference();
            
            if (!isServer) return;

            if (isAbleToStart)
            {
                Debug.Log("Im able to start countdown");
                StartCoroutine(StartCountdown());
                isAbleToStart = false;
            }
        }

        public IEnumerator StartCountdown()
        {
            if (!isServer) yield break;

            OnShootAppeared?.Invoke(false);

            countdownText.gameObject.SetActive(true);
            RpcShowCountdownOnClients();

            countdownText.text = Mathf.Ceil(time).ToString();
            serverTime = time;
            //yield return new WaitForSeconds(1f);

            while (time >= 1f)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log($"Countdown time: {time}");
                time -= 1f;
                countdownText.text = Mathf.Ceil(time).ToString();
                serverTime = time;
                //countdownText.text = ((int) time).ToString();
                //Debug.Log($"CurrentTime Timer: {currentTime}");
                //serverTime = time;
                //yield return new WaitForSecondsRealtime(1f);
            }
            
            if (time <= 0f)
            {
                time = 0f;
                // player.ShootingTime();
                // player.RpcAllowPlayersToShot();
                OnCountdownOver?.Invoke();
                Debug.Log("Players can shoot now");
            }

            float randomShootTime = UnityEngine.Random.Range(0.5f, 4f);
            serverRandomTime = randomShootTime;
            //Debug.Log($"Random time to shoot: {randomShootTime}");

            if (randomShootTime >= 1)
            {
                float randomDecimals = randomShootTime % (int) randomShootTime;

                while (randomShootTime > 1f)
                {
                    Debug.Log($"Random time to shoot: {randomShootTime}");
                    randomShootTime -= 1;
                    serverRandomTime = randomShootTime;
                    yield return new WaitForSeconds(1f);
                }

                Debug.Log($"Random time to shoot: {randomShootTime}");
                randomShootTime -= randomDecimals;
                yield return new WaitForSeconds(randomDecimals);
                Debug.Log($"Random time to shoot: {randomShootTime}");
                serverRandomTime = randomShootTime;
            }
            else
            {
                Debug.Log($"Random time to shoot: {randomShootTime}");
                randomShootTime = 0;
                yield return new WaitForSeconds(randomShootTime);
                Debug.Log($"Random time to shoot: {randomShootTime}");
                serverRandomTime = randomShootTime;
            }

            shootLabel.SetActive(true);
            RpcShowShootLabelOnClients();

            OnShootAppeared?.Invoke(true);

            countdownText.gameObject.SetActive(false);
            RpcHideCountdownOnClients();

            time = startingTime;
            serverTime = time;
        }

        [Command(requiresAuthority = false)]
        public void CmdStartCountdown()
        {
            StartCoroutine(StartCountdown());
            Debug.Log("Coroutine started by player 1");
        }

        [ClientRpc]
        private void RpcShowCountdownOnClients()
        {
            countdownText.gameObject.SetActive(true);
            Debug.Log("Activating countdown text on clients");
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

        public void EnableCountdownStart()
        {
            isAbleToStart = true;
        }
        
        private void ObtainPlayerReference()
        {
            if (!player)
            {
                GameObject player1 = GameObject.Find("Player 1");

                if (player1)
                {
                    player = player1.GetComponent<PlayerShootOnline>();
                }
                
            }
        }
    }
}
