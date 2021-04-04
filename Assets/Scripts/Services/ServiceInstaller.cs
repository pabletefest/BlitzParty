using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceInstaller : MonoBehaviour
{
    private void Awake()
    {
        var chronometerService = new ChronometerService();
        chronometerService.SetMonobehaviour(this);
        chronometerService.SetTimeInSeconds(30f); //Initial 30 seconds Countdown

        ServiceLocator.Instance.RegisterService<ITimer>(chronometerService);

        SceneManager.LoadScene("RabbitPursuit", LoadSceneMode.Additive);
    }
}
