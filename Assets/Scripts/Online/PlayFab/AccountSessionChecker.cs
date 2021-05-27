using UnityEngine;
using UnityEngine.SceneManagement;

namespace Online.PlayFab
{
    public class AccountSessionChecker : MonoBehaviour
    {
        [SerializeField] private Database localDatabase;
        
        private void Awake()
        {
            int isAcccountActive = localDatabase.GetAccountActiveToken();

            if (isAcccountActive == 1)
            {
                LoadScene("MainMenu");
            }
            else
            {
                LoadScene("Login");
            }
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
