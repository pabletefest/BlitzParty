using System;
using System.Collections;
using System.Security.Cryptography;
using Mirror;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Online.BinkyPursuit
{
    public class RabbitHideOnline: MonoBehaviour
    {
        public static event Action<GameObject> OnEnemyHidden;
        private IObjectPooler objectPooler;
        private bool isSpawnHole;
        
        public bool IsAlive { get; set; }

        private void Awake()
        {
            //objectPooler = ServiceLocator.Instance.GetService<IObjectPooler>();
            isSpawnHole = true;
            IsAlive = true;
        }

        private void OnEnable()
        {
            StartCoroutine(DisableColliderSpawnHole());        
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Hole") && !isSpawnHole)
            {
                //OnEnemyHidden?.Invoke(gameObject);
                //objectPooler.DisableObject(gameObject.name, SceneManager.GetActiveScene().name);
                //CmdUnspawnOverNetwork();
                /*if(gameObject)
                    Destroy(gameObject);*/
                Destroy(gameObject);
            }
        }

        public void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        /*[Command(requiresAuthority = false)]
        private void CmdUnspawnOverNetwork()
        {
            RpcDestroyOnClients();
            
            if(gameObject)
                Destroy(gameObject);
            //NetworkServer.UnSpawn(gameObject);
        }

        [ClientRpc]
        private void RpcDestroyOnClients()
        {
            if(gameObject)
                Destroy(gameObject);
        }*/

        private IEnumerator DisableColliderSpawnHole()
        {
            isSpawnHole = true;

            yield return new WaitForSeconds(1f);

            isSpawnHole = false;
        }
    }
}
