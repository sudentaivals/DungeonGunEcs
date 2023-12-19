using Leopotam.EcsLite;
using System;
public class NpcMovementSystem : IEcsInitSystem, IEcsDestroySystem
{
    private EcsPool<NpcMovement> _npcMovementPool;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeNpcMovement, ChangeNpcMovement);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _npcMovementPool = world.GetPool<NpcMovement>();
        EcsEventBus.Subscribe(GameplayEventType.ChangeNpcMovement, ChangeNpcMovement);
    }

    private void ChangeNpcMovement(int senderEntity, EventArgs args)
    {
        var npcMovementArgs = args as SetNpcMovementStatusEventArgs;
        ref var npcMovementComponent = ref _npcMovementPool.Get(senderEntity);
        npcMovementComponent.IsMovementActive = npcMovementArgs.NewNpcMovementStatus;
    }
}
