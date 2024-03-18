using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class TimedActionSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private List<TimedActionData> _actionData;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<PooledObjectTag> _pooledTagPool;
    public void Destroy(IEcsSystems systems)
    {
        _actionData.Clear();
        _actionData = null;
        EcsEventBus.Unsubscribe(GameplayEventType.RegisterTimedAction, RegisterTimedAction);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _actionData = new();
        _transformPool = world.GetPool<TransformComponent>();
        _pooledTagPool = world.GetPool<PooledObjectTag>();
        EcsEventBus.Subscribe(GameplayEventType.RegisterTimedAction, RegisterTimedAction);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var timedActionData in _actionData)
        {
            timedActionData.Timer -= Time.deltaTime;
            if(timedActionData.Timer > 0) continue;
            if(_pooledTagPool.Has(timedActionData.SenderEntity)) continue;
            if(!_transformPool.Has(timedActionData.SenderEntity)) continue;
            timedActionData.Action.Action(timedActionData.SenderEntity, timedActionData.TakerEntity);
        }
        _actionData.RemoveAll(a => a.Timer <= 0);
    }

    private void RegisterTimedAction(int sender, EventArgs args)
    {
        var registerTimedActionArgs = args as RegisterTimedActionEventArgs;
        var newActionData = new TimedActionData {Timer = registerTimedActionArgs.Timer, Action = registerTimedActionArgs.Action,
        SenderEntity = sender, TakerEntity = registerTimedActionArgs.TakerEntity};
        _actionData.Add(newActionData);
    }
}

public class TimedActionData
{
    public float Timer;
    public GameAction Action;
    public int SenderEntity;
    public int? TakerEntity;
}
