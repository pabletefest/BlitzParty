using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Online.CowboyDuel
{
    public class GameFinisherOnline : NetworkBehaviour
    {
        public event Action OnGameEnd;

        [SerializeField] private WinnerCheckerOnline winnerChecker;
        [SerializeField] private PanelHandlerOnline finalPanel;

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
            if (!isServer) return;
            
            OnGameEnd?.Invoke();
            finalPanel.ShowCowboyDuelPanel();
        }

        public void GameRestarter()
        {
            SceneManager.LoadScene("CowboyDuel");
        }
    }
}
