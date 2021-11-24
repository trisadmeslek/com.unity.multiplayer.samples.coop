using Unity.Netcode;
using UnityEngine;

public class ServerActionHandler : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
        }

        var playerInputs = GetComponents<PlayerInput>();

        foreach (var playerInput in playerInputs)
        {
            playerInput.clientActionRequested += TryPlayAction;
        }
    }

    void TryPlayAction(ActionScriptableObject obj)
    {

    }
}
