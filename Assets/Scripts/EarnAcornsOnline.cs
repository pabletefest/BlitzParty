using System;
using UnityEngine;

public class EarnAcornsOnline : MonoBehaviour
{

    [SerializeField]
    private PlayersScoreOnline playersScore;

    [SerializeField]
    private Database database;

    public void AcornsRabbitPursuit(int acornsEarned)
    {
        UpdateAcorns(acornsEarned);
    }

    public void AcornsWhackAMole(int acornsEarned)
    {
        UpdateAcorns(acornsEarned);
    }

    public void AcornsCowboyDuel(int acornsEarned)
    {
        UpdateAcorns(acornsEarned);
    }

    private void UpdateAcorns(int acornsEarned)
    {
        database.SaveAcorns(database.LoadAcorns() + acornsEarned);
    }

     public int CalculateAcornsEarned(string minigame, int playerNumber)
    {
        int moreAcorns = 0;
        switch (minigame)
        {
            case "RabbitPursuit":
                if (playerNumber == 1)
                {
                    moreAcorns = (int)Math.Floor(playersScore.GetP1Score() * 0.5);
                }
                else if (playerNumber == 2)
                {
                    moreAcorns = (int)Math.Floor(playersScore.GetP2Score() * 0.5);
                }
                
                break;

            case "WhackAMole":
                if (playerNumber == 1)
                {
                    moreAcorns = (int)Math.Floor(playersScore.GetP1Score() * 0.25);
                }
                else if (playerNumber == 2)
                {
                    moreAcorns = (int)Math.Floor(playersScore.GetP2Score() * 0.25);
                }
                
                break;

            case "CowboyDuel":
                moreAcorns = 10;
                
                break;
        }
        
        int acornsEarned = 20 + moreAcorns + 2000;

        return acornsEarned;
    }
}
