using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Services;

namespace WhackAMole
{
    public class HammerSpawner : MonoBehaviour, ITool
    {

        private Camera mainCamera;

        [SerializeField]
        private GameObject hammerPrefab;

        private float hitTime;
        private float hitRate = 0.2f;
        private bool hammerInUse;

        public void PerformAction()
        {
            if (Input.touchCount > 0 && !hammerInUse)
            {
                hammerInUse = true;
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = GetTouchPosition(touch);
                ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("HammerSwing");
                Instantiate(hammerPrefab, touchPosition, Quaternion.identity);
                StartCoroutine(FreeHammer());
            }

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

        // Update is called once per frame
        void Update()
        {
            PerformAction();
        }

        private IEnumerator FreeHammer()
        {
            yield return new WaitForSeconds(0.2f);
            hammerInUse = false;
        }
    }
}

