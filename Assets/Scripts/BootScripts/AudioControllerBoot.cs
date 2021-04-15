using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerBoot : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
