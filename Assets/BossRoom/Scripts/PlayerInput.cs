using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField]
    KeyCode m_KeyCode;

    public UnityEvent clientActionRequestedUnityEvent;

    public override void OnNetworkSpawn()
    {
        if (!IsClient || !IsOwner)
        {
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(m_KeyCode))
        {
            PlayerInputServerRpc();
        }
    }

    [ServerRpc]
    void PlayerInputServerRpc()
    {
        clientActionRequestedUnityEvent?.Invoke();
    }
}
