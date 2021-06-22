using System;
using Mirror;
using RabbitPursuit;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using CharacterController = RabbitPursuit.CharacterController;

namespace Online.BinkyPursuit
{
    public class ResetRabbitPursuitOnline : NetworkBehaviour
    {
        [SerializeField]
        private CharacterControllerOnline[] characterControllers;
  

        public override void OnStartClient()
        {
            characterControllers = FindObjectsOfType<CharacterControllerOnline>();
        }

        public void Reset()
        {
            foreach (var characterController in characterControllers)
            {
                characterController.ResetController();
            }
        }
    }
}
