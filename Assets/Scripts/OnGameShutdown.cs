using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Online.PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameShutdown : MonoBehaviour
{
    [SerializeField]
    private Database localDatabase;

    private bool isDataStored;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InvokeRepeating("StoreOnCloudUserData", 60f, 60f);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("Game paused");
            StoreOnCloudUserData();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Game exiting");
        StoreOnCloudUserData();
    }

    private void StoreOnCloudUserData()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int) SceneIndex.BootStrapper ||
            SceneManager.GetActiveScene().buildIndex == (int) SceneIndex.Login)
        {
            return;
        }
        
        string playFabId = localDatabase.GetPlayFabId();
        string username = localDatabase.GetUsername();
    
        int acorns = localDatabase.LoadAcorns();
    
        float musicVolume = localDatabase.LoadMusicVolume();
        float sfxVolume = localDatabase.LoadSFXVolume();
        GameSettings gameSettings = new GameSettings(musicVolume, sfxVolume);

        CloudStoragePlayFab cloudStorage = new CloudStoragePlayFab();

        cloudStorage.SetUserData(playFabId, username, acorns, gameSettings);
        Debug.Log("Storing data");
    
    }
}
