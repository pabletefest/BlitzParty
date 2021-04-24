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

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private Player2AI enemyController;

    private Vector2 p1OriginalPosition;
    private Vector2 p2OriginalPosition;

    private Vector3 p1OriginalScale;
    private Vector3 p2OriginalScale;

    private void Start()
    {
        p1OriginalPosition = p1Position.position;
        p2OriginalPosition = p2Position.position;
        p1OriginalScale = p1Position.localScale;
        p2OriginalScale = p2Position.localScale;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        scoreController.ResetScore();
        OnSceneRestarted?.Invoke(SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.GetService<ITimer>().ResetTimer();
        //joystick.SetActive(true);
        //joystick.GetComponent<CanvasRenderer>().SetAlpha(1f);
        //joystick.GetComponent<FloatingJoystick>().enabled = true;
        joystick.GetComponent<Canvas>().enabled = true;
        catchButton.SetActive(true);
        p1Position.position = p1OriginalPosition;
        p2Position.position = p2OriginalPosition;
        p1Position.localScale = p1OriginalScale;
        p2Position.localScale = p2OriginalScale;
        characterController.ResetController();
        enemyController.ResetVelocity();
    }
}
