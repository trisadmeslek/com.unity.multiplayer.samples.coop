using UnityEngine;

public abstract class ActionScriptableObject : ScriptableObject
{
    public ServerAction GetAction(ulong creatorNetworkObjectId)
    {
        var serverAction = CreateAction();
        serverAction.creatorNetworkObjectId = creatorNetworkObjectId;
        serverAction.originScriptableObject = this;
        serverAction.Awake();
        return serverAction;
    }

    protected abstract ServerAction CreateAction();
}
