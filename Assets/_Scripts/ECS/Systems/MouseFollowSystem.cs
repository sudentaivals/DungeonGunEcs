using Leopotam.EcsLite;
using UnityEngine;

public class MouseFollowSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<TransformComponent> _transformPool;
    public void Destroy(IEcsSystems systems)
    {
        //Cursor.visible = true;
    }

    public void Init(IEcsSystems systems)
    {
        //Cursor.visible = false;
        var world = systems.GetWorld();
        _filter = world.Filter<MouseWorldPositionFollowTag>().Inc<TransformComponent>().End();
        _transformPool = world.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var transformComponent = ref _transformPool.Get(entity);
            transformComponent.Transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
