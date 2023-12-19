using System;
using Leopotam.EcsLite;

public class CameraGroupSystem : IEcsInitSystem, IEcsDestroySystem
{

    private EcsPool<TransformComponent> _transformPool;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveSenderFromCinemachineGroup, RemoveSenderFromCinemachineTargetGroup);
        EcsEventBus.Unsubscribe(GameplayEventType.AddSenderToCinemachineGroup, AddSenderToCinemachineTargetGroup);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _transformPool = world.GetPool<TransformComponent>();
        EcsEventBus.Subscribe(GameplayEventType.RemoveSenderFromCinemachineGroup, RemoveSenderFromCinemachineTargetGroup);
        EcsEventBus.Subscribe(GameplayEventType.AddSenderToCinemachineGroup, AddSenderToCinemachineTargetGroup);

    }

    private void AddSenderToCinemachineTargetGroup(int entity, EventArgs args)
    {
        var cinemachineTargetArgs = args as AddSenderToCinemachineTargetGroupEventArgs;
        var transformComponent = _transformPool.Get(entity);
        CinemachineTarget.CinemachineTargetGroup.AddMember(transformComponent.Transform, cinemachineTargetArgs.Weight, cinemachineTargetArgs.Radius);
    }

    private void RemoveSenderFromCinemachineTargetGroup(int entity, EventArgs args)
    {
        var transformComponent = _transformPool.Get(entity);
        CinemachineTarget.CinemachineTargetGroup.RemoveMember(transformComponent.Transform);
    }
}
