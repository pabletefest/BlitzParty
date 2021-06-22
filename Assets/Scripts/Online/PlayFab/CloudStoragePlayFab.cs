using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Online.PlayFab
{
    public class CloudStoragePlayFab
    {
        public event Action<Dictionary<string, string>> OnDataReceived;
        public event Action OnDataStored;
        
        public CloudStoragePlayFab(){}

        public void SetUserData(string playFabId, string username, int acornsEarned, GameSettings gameSettings)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>()
                    {
                        {"PlayFabId", playFabId},
                        {"Username", username},
                        {"Acorns", acornsEarned.ToString()},
                        {"MusicVolume", gameSettings.MusicVolume.ToString()},
                        {"SFXVolume", gameSettings.SFXVolume.ToString()}
                    }
                },
                result =>
                {
                    Debug.Log("Successfully updated user data");
                    OnDataStored?.Invoke();
                },
                error => {
                    Debug.Log("Got error setting user data Ancestor to Arthur");
                    Debug.Log(error.GenerateErrorReport());
                });
            
        }
        
        public void SetNewUserData(string playFabId, string username, int acornsEarned)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>()
                    {
                        {"PlayFabId", playFabId},
                        {"Username", username},
                        {"Acorns", acornsEarned.ToString()},
                        {"MusicVolume", "1"},
                        {"SFXVolume", "1"}
                    }
                },
                result =>
                {
                    Debug.Log("Successfully updated user data");
                    OnDataStored?.Invoke();
                },
                error => {
                    Debug.Log("Got error setting user data Ancestor to Arthur");
                    Debug.Log(error.GenerateErrorReport());
                });
            
        }

        public void GetUserData(string playFabId)
        {
            PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
                PlayFabId = playFabId,
                Keys = null
            }, result => {
                Debug.Log("Got user data:");
                if (result.Data == null)
                {
                    Debug.Log("No data could be found!");
                    return;
                }

                var obtainedData = new Dictionary<string, string>()
                {
                    {"PlayFabId", result.Data["PlayFabId"].Value},
                    {"Username", result.Data["Username"].Value},
                    {"Acorns", result.Data["Acorns"].Value},
                    {"MusicVolume", result.Data["MusicVolume"].Value},
                    {"SFXVolume", result.Data["SFXVolume"].Value}
                };
                
                OnDataReceived?.Invoke(obtainedData);

            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }
    }
}
