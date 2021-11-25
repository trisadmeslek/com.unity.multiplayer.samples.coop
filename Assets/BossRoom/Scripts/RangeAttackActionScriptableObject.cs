using BossRoom.Scripts.Shared.Net.NetworkObjectPool;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.BossRoom.Server;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu]
public class RangeAttackActionScriptableObject : ActionScriptableObject
{
    public string animationTrigger;

    public NetworkObject projectile;

    protected override ServerAction CreateAction()
    {
        return new RangeAttackAction();
    }
}

public class RangeAttackAction : ServerAction
{
    RangeAttackActionScriptableObject OriginScriptableObject => (RangeAttackActionScriptableObject)base.originScriptableObject;

    public override void Awake()
    {
        // get player
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(creatorNetworkObjectId, out var creatorNetworkObject))
        {
            Debug.LogError("Owner potentially left game?");
            return;
        }

        // play animation
        if (creatorNetworkObject.TryGetComponent(out ServerAnimationHandler serverAnimationHandler))
        {
            serverAnimationHandler.NetworkAnimator.SetTrigger(OriginScriptableObject.animationTrigger);
        }

        var projectileNetworkObject = NetworkObjectPool.Singleton.GetNetworkObject(OriginScriptableObject.projectile.gameObject,
            OriginScriptableObject.projectile.transform.position, Quaternion.identity);

        // point the projectile the same way we're facing
        projectileNetworkObject.transform.forward = creatorNetworkObject.transform.forward;

        // this way, you just need to "place" the arrow by moving it in the prefab, and that will control where it
        // appears next to the player.
        projectileNetworkObject.transform.position = creatorNetworkObject.transform.localToWorldMatrix.MultiplyPoint(
            projectileNetworkObject.transform.position);

        var serverProjectileLogic = projectileNetworkObject.GetComponent<ServerProjectileLogic>();

        var projectileInfo = new ActionDescription.ProjectileInfo()
        {
            Damage = 5,
            MaxVictims = 1,
            ProjectilePrefab = null,
            Range = 20,
            Speed_m_s = 16
        };

        serverProjectileLogic.Initialize(creatorNetworkObjectId, projectileInfo);

        projectileNetworkObject.Spawn(true);
    }

    public override bool Update()
    {
        return false;
    }

    public override void End()
    {

    }
}
