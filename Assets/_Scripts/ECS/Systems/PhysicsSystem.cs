using System;
using Leopotam.EcsLite;

public class PhysicsSystem : IEcsInitSystem, IEcsDestroySystem
{ 
    private EcsPool<PhysicalBodyComponent> _physicalBodyPool;
    public void Destroy(IEcsSystems systems)
    {
         EcsEventBus.Unsubscribe(GameplayEventType.ChangeColliderStatus, ChangeColliderStatus);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ChangeColliderStatus, ChangeColliderStatus);
    }

    private void ChangeColliderStatus(int entity, EventArgs args)
    {
        if(!_physicalBodyPool.Has(entity)) return;
        var changeStatusArgs = args as ChangeStatusEventArgs;

        ref var physicalBody = ref _physicalBodyPool.Get(entity);

        physicalBody.PhysicalBody.enabled = changeStatusArgs.NewStatus;
    }
}
