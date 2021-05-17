using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleModeHandler : MonoBehaviour
{
    [SerializeField]
    private Database database;


    public void StartBattle()
    {
        database.ResetMinigames();
        database.SetIsBattleMode(true);
        StartNextMinigame();
    }

    public void StartNextMinigame()
    {
        int nextMinigameCounter = database.LoadCurrentBattleStage();
        if (nextMinigameCounter < 3)
        {
            List<string>  minigames = database.LoadMinigames();
            string nextMinigame = minigames[nextMinigameCounter];
            database.SaveCurrentBattleMinigame(nextMinigame);
            database.UpdateCurrentBattleStage();
            Debug.Log(minigames.Count);
            Debug.Log(minigames[0]);
            Debug.Log(minigames[1]);
            Debug.Log(minigames[2]);
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            //Battle finished -> Show final results
            database.SetIsBattleMode(false);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
