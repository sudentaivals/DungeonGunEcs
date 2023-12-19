using System;
using Leopotam.EcsLite;

public class GlobalStatsSystem : IEcsInitSystem, IEcsDestroySystem
{
    EcsPool<GlobalStatsComponent> _globalStatsPool;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.SetImmuneStatus, SetImmuneStatus);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _globalStatsPool = world.GetPool<GlobalStatsComponent>();
        EcsEventBus.Subscribe(GameplayEventType.SetImmuneStatus, SetImmuneStatus);
    }

    private void SetImmuneStatus(int entity, EventArgs args)
    {
        var immuneArgs = args as ChangeImmuneStatusEventArgs;
        ref var globalStats = ref _globalStatsPool.Get(entity);
        globalStats.IsImmune = immuneArgs.IsImmune;
    }
}
