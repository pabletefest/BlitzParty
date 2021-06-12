using System;
using UnityEngine;

namespace Online
{
    public class NetworkTypeChecker : MonoBehaviour
    {
        private NetworkType netType = NetworkType.OFF;
        public NetworkType NetType => netType;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SelectNetworkType(int type)
        {
            netType = (NetworkType) type;
        }
    }
}
