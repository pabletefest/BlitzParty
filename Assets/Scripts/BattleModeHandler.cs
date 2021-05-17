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
