using System.Collections;
using Mirror;
using Services;
using UnityEngine;

namespace Online.WhackAMole
{
    public class HammerSpawnerOnline : NetworkBehaviour, ITool
    {
        private Camera mainCamera;

        [SerializeField]
        private GameObject hammerPrefab;

        private float hitTime;
        private float hitRate = 0.2f;
        private bool hammerInUse;

        public int PlayerNumber { get; set; }

        public void PerformAction()
        {
            if (Input.touchCount > 0 && !hammerInUse)
            {
                hammerInUse = true;
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = GetTouchPosition(touch);
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HammerSwing");
                SpawnHammerOnNetwork(touchPosition, Quaternion.identity);
                StartCoroutine(FreeHammer());
            }

        }

        private void PlayerInputClick()
        {
            Vector3 clickPosition;
            bool clicked = CheckPlayerClick(out clickPosition);

            if (clicked && !hammerInUse)
            {
                hammerInUse = true;
                //ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HammerSwing");
                SpawnHammerOnNetwork(clickPosition, Quaternion.identity);
                StartCoroutine(FreeHammer());
            }
        }

        [Command]
        private void SpawnHammerOnNetwork(Vector3 clickPosition, Quaternion rotation)
        {
            Debug.Log(hammerPrefab);
            GameObject hammer = Instantiate(hammerPrefab, clickPosition, rotation);
            NetworkServer.Spawn(hammer, connectionToClient);
            hammer.GetComponent<HammerOnline>().SetPlayerOwner(gameObject);
        }
        
        private bool CheckPlayerClick(out Vector3 clickPosition)
        {
            clickPosition = Vector3.zero;
            
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                
                if (hit.collider.CompareTag("Background") || hit.collider.CompareTag("Mole") || hit.collider.CompareTag("GoldMole") || hit.collider.CompareTag("ZoomyWhackAMole"))
                {
                    clickPosition = hit.point;
                    Debug.Log($"clickedPosition: {clickPosition}");
                    Debug.Log($"collider tag hit: {hit.collider.tag}");
                    return true;
                    //Debug.Log("Player clicked the screen");
                }
            }

            return false;
        }

        private Vector3 GetTouchPosition(Touch touch)
        {
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 2;
            return touchPosition;
        }

        void Awake()
        {
            mainCamera = Camera.main;
            //hammerInUse = false;
        }


        [Client]
        void Update()
        {
            if (!isLocalPlayer) return;

            #if UNITY_EDITOR
                PlayerInputClick();
            #else
                PerformAction();
            #endif
        }

        private IEnumerator FreeHammer()
        {
            yield return new WaitForSeconds(hitRate);
            hammerInUse = false;
        }
    }
}

