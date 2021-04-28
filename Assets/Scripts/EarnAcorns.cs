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
        int moreAcorns = (int) Math.Floor(playersScore.GetP1Score() * 0.5);
        UpdateAcorns(AcornsEarned(moreAcorns));
    }

    public void AcornsWhackAMole()
    {
        int moreAcorns = (int) Math.Floor(playersScore.GetP1Score() * 0.25);
        UpdateAcorns(AcornsEarned(moreAcorns));
    }

    private void UpdateAcorns(int acornsEarned)
    {
        database.SaveUserAcorns(database.LoadUserAcorns() + acornsEarned);
    }

    private int AcornsEarned(int moreAcorns)
    {
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
