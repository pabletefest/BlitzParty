using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Services;

public class ResetRabbitPursuit : MonoBehaviour
{

    public static event Action<string> OnSceneRestarted;

    [SerializeField]
    private GameObject joystick;

    [SerializeField]
    private GameObject catchButton;

    [SerializeField]
    private PlayersScore scoreController;

    [SerializeField]
    private Transform p1Position;

    [SerializeField]
    private Transform p2Position;

    private Vector2 p1OriginalPosition;
    private Vector2 p2OriginalPosition;

    private void Start()
    {
        p1OriginalPosition = p1Position.position;
        p2OriginalPosition = p2Position.position;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        scoreController.ResetScore();
        OnSceneRestarted?.Invoke(SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
        joystick.SetActive(true);
        catchButton.SetActive(true);
        p1Position.position = p1OriginalPosition;
        p2Position.position = p2OriginalPosition;
    }
}
