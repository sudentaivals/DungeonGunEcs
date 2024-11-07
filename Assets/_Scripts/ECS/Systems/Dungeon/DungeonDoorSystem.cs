using System;
using Leopotam.EcsLite;

public class DungeonDoorSystem : IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;

    private EcsPool<DoorComponent> _doorPool;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.CloseAllDoors, CloseAllDoors);
        EcsEventBus.Unsubscribe(GameplayEventType.OpenAllDoors, OpenAllDoors);
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeDoorStatus, ChangeDoorStatus);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<DoorComponent>().Exc<PooledObjectTag>().End();
        _doorPool = world.GetPool<DoorComponent>();
        EcsEventBus.Subscribe(GameplayEventType.CloseAllDoors, CloseAllDoors);
        EcsEventBus.Subscribe(GameplayEventType.OpenAllDoors, OpenAllDoors);
        EcsEventBus.Subscribe(GameplayEventType.ChangeDoorStatus, ChangeDoorStatus);

    }

    private void ChangeDoorStatus(int sender, EventArgs args)
    {
        if(!_doorPool.Has(sender)) return;
        var doorArgs = args as ChangeDoorStatusEventArgs;

        ref var door = ref _doorPool.Get(sender);

        door.IsOpen = doorArgs.NewStatus;
    }

    private void OpenAllDoors(int sender, EventArgs args)
    {
        foreach (var entity in _filter)
        {
            ref var door = ref _doorPool.Get(entity);
            door.IsOpen = true;
        }
    }

    private void CloseAllDoors(int sender, EventArgs args)
    {
        foreach (var entity in _filter)
        {
            ref var door = ref _doorPool.Get(entity);
            door.IsOpen = false;
        }
    }
}
