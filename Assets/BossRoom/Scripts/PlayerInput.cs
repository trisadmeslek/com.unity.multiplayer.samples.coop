using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    [SerializeField]
    KeyCode m_KeyCode;

    [SerializeField]
    ActionScriptableObject m_ActionScriptableObject;

    public event Action<ActionScriptableObject> clientActionRequested;

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
        clientActionRequested?.Invoke(m_ActionScriptableObject);
    }
}
