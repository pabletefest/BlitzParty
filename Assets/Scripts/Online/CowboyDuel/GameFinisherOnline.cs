using UnityEngine;
using UnityEngine.SceneManagement;

namespace Online.CowboyDuel
{
    public class GameFinisheOnline : MonoBehaviour
    {

        [SerializeField] private WinnerCheckerOnline winnerChecker;
        [SerializeField] private PanelHandler finalPanel;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            winnerChecker.OnGameEnd += GameEnded;
        }

        private void OnDisable()
        {
            winnerChecker.OnGameEnd -= GameEnded;
        }

        public void GameEnded()
        {
            finalPanel.ShowCowboyDuelPanel();
        }

        public void GameRestarter()
        {
            SceneManager.LoadScene("CowboyDuel");
        }
    }
}
