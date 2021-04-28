using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInMenu : MonoBehaviour
{
    [SerializeField]
    Database database;

    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private Text errorText;

    public void SignInHandler()
    {
        string username = usernameText.text;
        if (!username.Equals(""))
        {
            User user = database.LoadUser(username);
            if (user != null)
            {
                GoToMainMenu();
            }
            else 
            {
                errorText.text = "User not found. Please try again.";
            }
        }
    }

    public void RegisterHandler()
    {
        int id = database.LoadTotalUsers() + 1;
        string username = usernameText.text;

        if (!username.Equals(""))
        {
            User user = new User(id, username);
            if (database.AddUser(user))
            {
                GoToMainMenu();
            }
            else 
            {
                errorText.text = "Username already in use.";
            }
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
