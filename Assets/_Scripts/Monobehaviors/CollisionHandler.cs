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
    private bool _triggered = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!_onTriggerStay) return;
        if (_triggered) return;
        TriggerCollisionEvents(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!_onTriggerExit) return;
        if (_triggered) return;
        TriggerCollisionEvents(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_onTriggerEnter) return;
        if (_triggered) return;
        TriggerCollisionEvents(other);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!_onCollisionEnter) return;
        if (_triggered) return;
        TriggerCollisionEvents(other.collider);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(!_onCollisionStay) return;
        if (_triggered) return;
        TriggerCollisionEvents(other.collider);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(!_onCollisionExit) return;
        if (_triggered) return;
        TriggerCollisionEvents(other.collider);
    }

    public void ResetTrigger()
    {
        _triggered = false;
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
            ref var collisionComp = ref collisionPool.Get(thisEntity);
            if(collisionComp.Condition == null) return;
            if(collisionComp.Action == null) return;
            var conditionValid = collisionComp.Condition.CheckCondition(thisEntity, otherEntity.Value);
            if (!conditionValid) return;
            _triggered = true;
            collisionComp.Action.Action(thisEntity, otherEntity.Value);
        }
    }
    

}
