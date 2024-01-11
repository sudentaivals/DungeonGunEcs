using Leopotam.EcsLite;
using UnityEngine;

public class FlipSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<TransformComponent> _transformPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MovementStatsComponent>()
                       .Exc<FlipIgnoreTag>()
                       .Exc<PooledObjectTag>()
                       .End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {

        foreach (int entity in _filter)
        {
            ref var movementStats = ref _movementStatsPool.Get(entity);
            if(movementStats.MovementDirection.x == 0f) continue;
            ref var transform = ref _transformPool.Get(entity);
            //flip
            if (Mathf.Abs(movementStats.MovementDirection.magnitude) >= float.Epsilon)
            {
                
                var scaleSign = Mathf.Sign(transform.Transform.localScale.x);
                var horizontalSign = Mathf.Sign(movementStats.MovementDirection.x);
                if (scaleSign == horizontalSign) continue;
                transform.Transform.localScale = new Vector3(
                    transform.Transform.localScale.x * -1,
                    transform.Transform.localScale.y,
                    transform.Transform.localScale.z);
            }
        }

    }
}
