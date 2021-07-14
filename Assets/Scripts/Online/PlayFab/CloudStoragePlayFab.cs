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

        public void SetUserData(string playFabId, string username, int acornsEarned, GameSettings gameSettings, int RabbitPursuitGames, int WhackAMoleGames, int CowboyDuelGames, int RabbitPursuitWins, int WhackAMoleWins, int CowboyDuelWins)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
                    Data = new Dictionary<string, string>()
                    {
                        {"PlayFabId", playFabId},
                        {"Username", username},
                        {"Acorns", acornsEarned.ToString()},
                        {"MusicVolume", gameSettings.MusicVolume.ToString()},
                        {"SFXVolume", gameSettings.SFXVolume.ToString()},
                        {"RabbitPursuitGames", RabbitPursuitGames.ToString()},
                        {"WhackAMoleGames", WhackAMoleGames.ToString()},
                        {"CowboyDuelGames", CowboyDuelGames.ToString()},
                        {"RabbitPursuitWins", RabbitPursuitWins.ToString()},
                        {"WhackAMoleWins", WhackAMoleWins.ToString()},
                        {"CowboyDuelWins", CowboyDuelWins.ToString()}
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
                        {"SFXVolume", "1"},
                        {"RabbitPursuitGames", "0"},
                        {"WhackAMoleGames", "0"},
                        {"CowboyDuelGames", "0"},
                        {"RabbitPursuitWins", "0"},
                        {"WhackAMoleWins", "0"},
                        {"CowboyDuelWins", "0"}
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
                    {"SFXVolume", result.Data["SFXVolume"].Value},
                    {"RabbitPursuitGames", result.Data.TryGetValue("RabbitPursuitGames", out UserDataRecord valueRabbitGames) ? valueRabbitGames.Value : "0" },
                    {"WhackAMoleGames", result.Data.TryGetValue("WhackAMoleGames", out UserDataRecord valueWhackGames) ? valueWhackGames.Value : "0"},
                    {"CowboyDuelGames", result.Data.TryGetValue("CowboyDuelGames", out UserDataRecord valueCowboyGames) ? valueCowboyGames.Value : "0"},
                    {"RabbitPursuitWins", result.Data.TryGetValue("RabbitPursuitWins", out UserDataRecord valueRabbitWins) ? valueRabbitWins.Value : "0"},
                    {"WhackAMoleWins", result.Data.TryGetValue("WhackAMoleWins", out UserDataRecord valueWhackWins) ? valueWhackWins.Value : "0"},
                    {"CowboyDuelWins", result.Data.TryGetValue("CowboyDuelWins", out UserDataRecord valueCowboyWins) ? valueCowboyWins.Value : "0"}
                };
                
                OnDataReceived?.Invoke(obtainedData);

            }, (error) => {
                Debug.Log("Got error retrieving user data:");
                Debug.Log(error.GenerateErrorReport());
            });
        }
    }
}
