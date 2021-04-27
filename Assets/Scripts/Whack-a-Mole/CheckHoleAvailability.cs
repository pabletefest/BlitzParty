using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHoleAvailability : MonoBehaviour
{

    private static CheckHoleAvailability instance;
    public static CheckHoleAvailability Instance => instance;
    private bool[] holeOccupied;
    private Dictionary<GameObject, bool> holesOccupied;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        holeOccupied = new bool[7];
    }

    public void occupyHole(int holeNumber)
    {
        holeOccupied[holeNumber] = true;
    }

    public bool isOccupied(int holeNumber)
    {
        return holeOccupied[holeNumber];
    }

    public void liberateHole(int holeNumber)
    {
        holeOccupied[holeNumber] = false;
    }

    public bool allOccupied()
    {
        bool allOccupied = true;

        for(int i = 0; i < holeOccupied.Length; i++)
        {
            if (!holeOccupied[i])
            {
                allOccupied = false;
            }
        }

        return allOccupied;
    }
}
