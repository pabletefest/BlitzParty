using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceInstaller : MonoBehaviour
{
    [SerializeField]
    private TimeChronometerSO timeChronometerSO;

    [SerializeField]
    private Sound[] sounds;

    [SerializeField]
    private AudioSource mainThemeSource;

    [SerializeField]
    private AudioSource soundFXSource;

    [SerializeField]
    private List<ObjectPool> pools;

    private void Awake()
    {
        var chronometerService = new ChronometerService(timeChronometerSO);
        chronometerService.SetMonobehaviour(this);

        ServiceLocator.Instance.RegisterService<ITimer>(chronometerService);
        
        var audioController = new AudioController(sounds, mainThemeSource, soundFXSource);

        ServiceLocator.Instance.RegisterService<ISoundAdapter>(audioController);

        var objectPoolerService = new ObjectPoolerService(pools);

        ServiceLocator.Instance.RegisterService<IObjectPooler>(objectPoolerService);

        //SceneManager.LoadScene("RabbitPursuit", LoadSceneMode.Additive);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
}
