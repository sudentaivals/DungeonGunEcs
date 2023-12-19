using System;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Conditions/Object is moving")]
public class ObjectIsMoving : BaseGameCondition
{
    [SerializeField] bool _isMovingExists;
    [SerializeField] bool _targetIsSender;
    private float MinSpeedTreshold => float.Epsilon;
    public override bool CheckCondition(int senderEntity, int? takerEntity)
    {
        var movementStatsPool = EcsStart.World.GetPool<MovementStatsComponent>();
        ref MovementStatsComponent movementStats = ref movementStatsPool.Get(senderEntity);
        if(!_targetIsSender)
        {
            if(takerEntity == null) return false;
            movementStats = ref movementStatsPool.Get(takerEntity.Value);
        }
        if(_isMovingExists) return movementStats.Movement.magnitude > MinSpeedTreshold;
        else return movementStats.Movement.magnitude <= MinSpeedTreshold;
    }
}
