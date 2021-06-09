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
        [SerializeField] private Text errorText;

        [SerializeField] private Database localDatabase;

        //sessionTicket for online multiplayer
        public static string SessionTicket;
        public static string PlayFabId;
    
        public void LoginPlayer()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                Username = usernameField.text,
                Password = passwordField.text

            }, result =>
            {
                SessionTicket = result.SessionTicket;
                PlayFabId = result.PlayFabId;
                localDatabase.SetAccountActiveToken(1);
                StoreUserDataLocalDB(PlayFabId, usernameField.text, passwordField.text);
                LoadMainMenu();
            
            }, error =>
            {
                ShowLoginErrorMessage();
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
                PlayFabId = result.PlayFabId;
                localDatabase.SetAccountActiveToken(1);
                StoreUserDataLocalDB(PlayFabId, result.Username, passwordField.text);
                LoadMainMenu();
            
            }, error =>
            {
                ShowRegisterErrorMessage();
                Debug.LogError(error.GenerateErrorReport());
            });
        }

        public void LogoutPlayer()
        {
            StoreUserDataOnCloud();
            PlayFabClientAPI.ForgetAllCredentials();
            SessionTicket = default;
            PlayFabId = default;
            localDatabase.SetAccountActiveToken(0);
            LoadLoginScene();
        }

        private void LoadMainMenu()
        {
            Debug.Log("Loading MainMenu");
            SceneManager.LoadScene("MainMenu");
        }

        private void LoadLoginScene()
        {
            Debug.Log("Loading Login");
            SceneManager.LoadScene("Login");
        }

        private void ShowRegisterErrorMessage()
        {
            if (emailField.text == string.Empty || usernameField.text == string.Empty ||
                passwordField.text == string.Empty)
            {
                errorText.text = "Please, check that there are no empty fields!";
            }
            else if (passwordField.text.Length < 6 || passwordField.text.Length > 100)
            {
                errorText.text = "Password must be at least 6 characters!";
            }
            else
            {
                errorText.text = "The account you are trying to register could be already in use!";
            }
        }

        private void ShowLoginErrorMessage()
        {
            if (usernameField.text == string.Empty ||
                passwordField.text == string.Empty)
            {
                errorText.text = "Please, check that there are no empty fields!";
            }
            else if (passwordField.text.Length < 6 || passwordField.text.Length > 100)
            {
                errorText.text = "Password must be at least 6 characters!";
            }
            else
            {
                errorText.text = "The account you are trying to login is not registered yet!";
            }
        }

        private void StoreUserDataLocalDB(string playFabId, string username, string password)
        {
            localDatabase.SetPlayFabId(playFabId);
            localDatabase.SetUsername(username);
            localDatabase.SetPassword(password);
        }

        private void StoreUserDataOnCloud()
        {
            string playFabId = localDatabase.GetPlayFabId();
            string username = localDatabase.GetUsername();
        
            int acorns = localDatabase.LoadAcorns();
        
            float musicVolume = localDatabase.LoadMusicVolume();
            float sfxVolume = localDatabase.LoadSFXVolume();
            GameSettings gameSettings = new GameSettings(musicVolume, sfxVolume);

            CloudStoragePlayFab cloudStorage = new CloudStoragePlayFab();
        
            cloudStorage.SetUserData(playFabId, username, acorns, gameSettings);
        }
    }
}
