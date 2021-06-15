using System;
using UnityEngine;

namespace Online
{
    public class NetworkTypeChecker : MonoBehaviour
    {
        public static NetworkTypeChecker Instance => instance;
        private static NetworkTypeChecker instance;
        
        private NetworkType netType = NetworkType.OFF;
        public NetworkType NetType => netType;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else 
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        public void SelectNetworkType(int type)
        {
            netType = (NetworkType) type;
        }
    }
}
