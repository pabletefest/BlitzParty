using Mirror;
using UnityEngine;

namespace Online.BinkyPursuit
{
    public class HookCatchButtonPlayer : NetworkBehaviour
    {
        [SerializeField]
        private PanelHandlerOnline panelHandler;

        public override void OnStartClient()
        {
            panelHandler = GameObject.Find("GUIController").GetComponent<PanelHandlerOnline>();
            panelHandler.AnchorCatchButtonToPlayer(GetComponent<PlayerMovementOnline>().CatchButtonHandler);
        }
    }
}
