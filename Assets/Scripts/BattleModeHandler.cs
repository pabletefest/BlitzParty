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
        List<string> minigames = database.RandomizeMinigames();
        StartNextMinigame();
    }

    public void StartNextMinigame()
    {
        int nextMinigameCounter = database.LoadCurrentBattleStage();
        if (nextMinigameCounter < 3)
        {
            string nextMinigame = "";
            if (nextMinigameCounter == 0)
            {
                nextMinigame = database.LoadMinigame1();
            }
            else if (nextMinigameCounter == 1)
            {
                nextMinigame = database.LoadMinigame2();
            }
            else
            {
                nextMinigame = database.LoadMinigame3();
            }
            database.SaveCurrentBattleMinigame(nextMinigame);
            database.UpdateCurrentBattleStage();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            //Battle finished -> Show final results
            database.SetIsBattleMode(false);
            database.ResetMinigames();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
