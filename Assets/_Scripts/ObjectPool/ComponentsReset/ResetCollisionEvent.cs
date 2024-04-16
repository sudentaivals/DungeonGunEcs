using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset collision event")]
public class ResetCollisionEvent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        var entity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity();
        if (entity == null) return;
        var world = EcsStart.World;
        var collisionPool = world.GetPool<CollisionEventsComponent>();
        if(!collisionPool.Has(entity.Value)) return;
        ref var collisionComponent = ref collisionPool.Get(entity.Value);
        collisionComponent.Triggered = false;
        collisionComponent.CurrentNumberOfCollisions = collisionComponent.BaseNumberOfCollisions;
    }
}

