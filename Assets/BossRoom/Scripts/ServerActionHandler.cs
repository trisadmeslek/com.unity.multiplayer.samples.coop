using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerActionHandler : NetworkBehaviour
{
    [SerializeField]
    ActionScriptableObject m_ActionScriptableObject;

    List<ServerAction> m_ServerActions = new List<ServerAction>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
        }
    }

    public void Execute()
    {
        if (m_ActionScriptableObject)
        {
            m_ServerActions.Add(m_ActionScriptableObject.GetAction(NetworkObjectId));
        }
    }

    void Update()
    {
        for (int i = m_ServerActions.Count - 1; i >= 0; i--)
        {
            var serverAction = m_ServerActions[i];
            var isActionRunning = serverAction.Update();

            if (!isActionRunning)
            {
                serverAction.End();
                m_ServerActions.RemoveAt(i);
            }
        }
    }
}
