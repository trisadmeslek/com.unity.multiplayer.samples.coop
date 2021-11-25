using UnityEngine;

public abstract class ServerAction : IActionable
{
    internal ulong creatorNetworkObjectId;

    internal ActionScriptableObject originScriptableObject;

    public abstract void Awake();

    public abstract bool Update();

    public abstract void End();
}
