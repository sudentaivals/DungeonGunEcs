using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voody.UniLeo.Lite;

public class CollisionHandler : MonoBehaviour
{
    private bool _triggered = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_triggered) return;
        if(other.TryGetComponent<ConvertToEntity>(out var converToEntity))
        {
            //get entities
            if (!gameObject.GetComponent<ConvertToEntity>().TryGetEntity().HasValue) return;
            if (!other.GetComponent<ConvertToEntity>().TryGetEntity().HasValue) return;
            int thisEntity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity().Value;
            var otherEntity = converToEntity.TryGetEntity();

            //get collisionComponent
            var collisionPool = EcsStart.World.GetPool<CollisionEventsComponent>();
            ref var collisionComp = ref collisionPool.Get(thisEntity);

            var conditionValid = collisionComp.Condition.CheckCondition(thisEntity, otherEntity.Value);
            if (!conditionValid) return;
            _triggered = true;
            collisionComp.Action.Action(thisEntity, otherEntity.Value);
        }
    }

    public void ResetTrigger()
    {
        _triggered = false;
    }

}
