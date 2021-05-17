using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinisher : MonoBehaviour
{

    [SerializeField] private WinnerChecker winnerChecker;
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
}
