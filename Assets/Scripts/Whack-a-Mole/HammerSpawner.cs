using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSpawner : MonoBehaviour, ITool
{

    private Camera mainCamera;

    [SerializeField]
    private GameObject hammerPrefab;

    public void PerformAction()
    {
        if (Input.touchCount > 0)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = GetTouchPosition(touch);
            Instantiate(hammerPrefab, touchPosition, Quaternion.identity);
            //}
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
