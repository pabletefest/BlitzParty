using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, ITool
{

    private float hitTime;
    private float hitRate = 0.5f;
    private Camera mainCamera;

    [SerializeField]
    private GameObject hammerPrefab;

    public void PerformAction()
    {
        if (Input.touchCount > 0) 
        {
            Vector3 touchPosition = GetTouchPosition();
            Instantiate(hammerPrefab, touchPosition, Quaternion.identity);
        }
        
    }

    private Vector3 GetTouchPosition()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        return touchPosition;
    }

    void Awake()
    {
        hitTime = hitRate;
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hitTime <= 0)
        {
            PerformAction();
            hitTime = hitRate;
        }
        else
        {
            hitTime -= Time.deltaTime;
        }
    }
}
