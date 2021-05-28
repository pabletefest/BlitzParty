using UnityEngine;
using UnityEngine.UI;

public class ProfileMenuHandler : MonoBehaviour
{
    [SerializeField]
    private Database database;

    [SerializeField]
    private Text username;

    [SerializeField]
    private Text rabbitPursuitGames;

    [SerializeField]
    private Text rabbitPursuitWins;

    [SerializeField]
    private Text whackAMoleGames;

    [SerializeField]
    private Text whackAMoleWins;

    [SerializeField]
    private Text cowboyDuelGames;

    [SerializeField]
    private Text cowboyDuelWins;

    void Start()
    {
        //UpdateProfileData();
    }

    public void UpdateProfileData(string usernameCloud)
    {
        if (usernameCloud == null)
            return;
        
        //username.text = database.LoadUsername();
        username.text = usernameCloud;

        rabbitPursuitGames.text = database.LoadPlayerRabbitPursuitGames().ToString();
        rabbitPursuitWins.text = database.LoadPlayerRabbitPursuitWins().ToString();

        whackAMoleGames.text = database.LoadPlayerWhackAMoleGames().ToString();
        whackAMoleWins.text = database.LoadPlayerWhackAMoleWins().ToString();

        cowboyDuelGames.text = database.LoadPlayerCowboyDuelGames().ToString();
        cowboyDuelWins.text = database.LoadPlayerCowboyDuelWins().ToString();
    }
}
