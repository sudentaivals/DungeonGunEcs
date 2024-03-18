using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/From master to sender")]
public class RotateFromMasterToSender : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        if(taker == null) return Quaternion.identity;
        var world = EcsStart.World;
        var summonedObjectPool = world.GetPool<SummonedObjectComponent>();
        if(!summonedObjectPool.Has(sender)) { return Quaternion.identity; }

        var transformPool = world.GetPool<TransformComponent>();
        ref var summonedObject = ref summonedObjectPool.Get(sender);
        if(!transformPool.Has(summonedObject.MasterId)) { return Quaternion.identity; }

        ref var masterTransform = ref transformPool.Get(summonedObject.MasterId);
        ref var senderTransform = ref transformPool.Get(sender);
        var masterToSender = senderTransform.Transform.position - masterTransform.Transform.position;
        var angle = Mathf.Atan2(masterToSender.y, masterToSender.x);
        var angleDeg = angle * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angleDeg);
    }
}
