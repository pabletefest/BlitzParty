using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Online.PlayFab
{
    public class PlayFabLogin : MonoBehaviour
    {
        [SerializeField] private InputField emailField;
        [SerializeField] private InputField usernameField;
        [SerializeField] private InputField passwordField;

        //sessionTicket for online multiplayer
        public static string SessionTicket;
    
        public void LoginPlayer()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                Username = usernameField.text,
                Password = passwordField.text

            }, result =>
            {
                SessionTicket = result.SessionTicket;
                LoadMainMenu();
            
            }, error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            });
        }

        public void RegisterPlayer()
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
            {
                Email = emailField.text,
                Username = usernameField.text,
                Password = passwordField.text

            }, result =>
            {
                SessionTicket = result.SessionTicket;
                LoadMainMenu();
            
            }, error =>
            {
                Debug.LogError(error.GenerateErrorReport());
            });
        }

        public void LogoutPlayer()
        {
            PlayFabClientAPI.ForgetAllCredentials();
            SessionTicket = default;
            LoadLoginScene();
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void LoadLoginScene()
        {
            SceneManager.LoadScene("Login");
        }
    }
}
