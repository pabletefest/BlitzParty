using System;
using UnityEngine;

public class EarnAcorns : MonoBehaviour
{

    [SerializeField]
    private PlayersScore playersScore;

    [SerializeField]
    private Database database;

    public void AcornsRabbitPursuit()
    {
        UpdateAcorns(CalculateAcornsEarned("RabbitPursuit"));
    }

    public void AcornsWhackAMole()
    {
        UpdateAcorns(CalculateAcornsEarned("WhackAMole"));
    }

    public void AcornsCowboyDuel()
    {
        UpdateAcorns(CalculateAcornsEarned("CowboyDuel"));
    }

    private void UpdateAcorns(int acornsEarned)
    {
        database.SaveAcorns(database.LoadAcorns() + acornsEarned);
    }

     public int CalculateAcornsEarned(string minigame)
    {
        int moreAcorns = 0;
        switch (minigame)
        {
            case "RabbitPursuit":
                moreAcorns = (int)Math.Floor(playersScore.GetP1Score() * 0.5);
                break;

            case "WhackAMole":
                moreAcorns = (int)Math.Floor(playersScore.GetP1Score() * 0.25);
                break;

            case "CowboyDuel":
                moreAcorns = 10;
                break;
        }
        int acornsEarned = 0;
        if (playersScore.FindWinner() == Results.DRAW)
        {
            acornsEarned = 10;
        }
        else if (playersScore.FindWinner() == Results.PLAYER1WIN)
        {
            acornsEarned = 20 + moreAcorns;
        }
        return acornsEarned;
    }
}
