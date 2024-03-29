using System.Net.WebSockets;
using Leopotam.EcsLite;

public class PlayerUseObjectsSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<PlayerInputComponent> _playerInputPool;
    public void Destroy(IEcsSystems systems)
    {
        return;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerInputComponent>().Inc<PickupComponent>().Exc<PooledObjectTag>().End();
        _playerInputPool = world.GetPool<PlayerInputComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var playerInput = ref _playerInputPool.Get(entity);
            if (!playerInput.UseObject) continue;
            EcsEventBus.Publish(GameplayEventType.UseObject, entity, null);
        }
    }
}
