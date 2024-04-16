using UnityEngine;
using Voody.UniLeo.Lite;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private bool _onTriggerEnter;
    [SerializeField] private bool _onTriggerStay = true;
    [SerializeField] private bool _onTriggerExit;
    [Header("Collision Events")]
    [SerializeField] private bool _onCollisionEnter;
    [SerializeField] private bool _onCollisionStay;
    [SerializeField] private bool _onCollisionExit;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!_onTriggerStay) return;
        TriggerCollisionEvents(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!_onTriggerExit) return;
        TriggerCollisionEvents(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_onTriggerEnter) return;
        TriggerCollisionEvents(other);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!_onCollisionEnter) return;
        TriggerCollisionEvents(other.collider);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(!_onCollisionStay) return;
        TriggerCollisionEvents(other.collider);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(!_onCollisionExit) return;
        TriggerCollisionEvents(other.collider);
    }

    private void TriggerCollisionEvents(Collider2D other)
    {
        if(other.TryGetComponent<ConvertToEntity>(out var converToEntity))
        {
            //get entities
            if (!gameObject.GetComponent<ConvertToEntity>().TryGetEntity().HasValue) return;
            if (!other.GetComponent<ConvertToEntity>().TryGetEntity().HasValue) return;
            int thisEntity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity().Value;
            var otherEntity = converToEntity.TryGetEntity();

            //get collisionComponent
            var collisionPool = EcsStart.World.GetPool<CollisionEventsComponent>();
            ref var thisCollisionComp = ref collisionPool.Get(thisEntity);
            if(thisCollisionComp.Triggered) return;
            if(thisCollisionComp.Condition == null) return;
            if(thisCollisionComp.Action == null) return;
            var conditionValid = thisCollisionComp.Condition.CheckCondition(thisEntity, otherEntity.Value);
            if (!conditionValid) return;
            if(!thisCollisionComp.IsInfinite) thisCollisionComp.CurrentNumberOfCollisions--;
            if(thisCollisionComp.CurrentNumberOfCollisions <= 0) thisCollisionComp.Triggered = true;
            thisCollisionComp.Action.Action(thisEntity, otherEntity.Value);
        }
    }
    

}
