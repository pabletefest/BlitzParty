using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, ITool
{

    private float hitTime;
    private float hitRate = 0.5f;
    private bool hitPossible;
    private Camera mainCamera;

    [SerializeField]
    private GameObject hammerPrefab;

    public void PerformAction()
    {
        Vector3 touchPosition = GetTouchPosition();
        Instantiate(hammerPrefab, touchPosition, Quaternion.identity);
    }

    private Vector3 GetTouchPosition()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        return touchPosition;
    }

    private void OnMouseDown()
    {
        if (hitPossible)
        {
            PerformAction();
            hitPossible = false;
        }
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
        if (hitTime <= 0)
        {
            hitPossible = true;
            hitTime = hitRate;
        }
        else
        {
            hitTime -= Time.deltaTime;
        }
    }
}