using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Sender is touching target")]
public class SenderIsTouchingTarget : BaseGameCondition
{
    [SerializeField] bool _isTouchingTarget;

    public override bool CheckCondition(int senderEntity, int? takerEntity)
    {
        var targetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTarget = ref targetPool.Get(senderEntity);
        if (!npcTarget.IsTargetFound) return false;

        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var senderTransform = ref transformPool.Get(senderEntity);

        var psysicalBodyPool = EcsStart.World.GetPool<PhysicalBodyComponent>();
        ref var senderPhysicsBody = ref psysicalBodyPool.Get(senderEntity);
        ref var targetPhysicsBody = ref psysicalBodyPool.Get(npcTarget.TargetEntity);
        ref var targetTransform = ref transformPool.Get(npcTarget.TargetEntity);

        var distance = Mathf.Abs((senderTransform.Transform.position.x - targetTransform.Transform.position.x)
                                *(senderTransform.Transform.position.x - targetTransform.Transform.position.x)
                                +(senderTransform.Transform.position.y - targetTransform.Transform.position.y)
                                *(senderTransform.Transform.position.y - targetTransform.Transform.position.y));
        var radiusSum = (senderPhysicsBody.BodyRadius + targetPhysicsBody.BodyRadius) * (senderPhysicsBody.BodyRadius + targetPhysicsBody.BodyRadius);
        bool isToching = distance <= radiusSum;
        if(_isTouchingTarget) return isToching == true;
        else return isToching == false;
    }
}
