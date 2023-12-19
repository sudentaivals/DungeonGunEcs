using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class StartEventsInvokerSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<EventsComponent> _eventsPool;
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _eventsPool = null;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<EventsComponent>().End();
        _eventsPool = world.GetPool<EventsComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var eventsComponent = ref _eventsPool.Get(entity);
            if (eventsComponent.StartEventsInvoked) continue;
            foreach (var action in eventsComponent.OnStart)
            {
                action.Action(entity, null);
            }
            eventsComponent.StartEventsInvoked = true;
        }
    }
}
