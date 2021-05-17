using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModeHandler : MonoBehaviour
{
    [SerializeField]
    private Database database;

    private List<string> minigames;

    public void StartBattle()
    {
        database.ResetMinigames();
        minigames = database.LoadMinigames();
        string nextMinigame = minigames[database.LoadCurrentBattleStage()];
        database.UpdateCurrentBattleStage();
    }
}
