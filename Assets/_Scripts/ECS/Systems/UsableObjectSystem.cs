using System;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class UsableObjectSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<PickupComponent> _pickupPool;
    private EcsPool<UsableObjectComponent> _usableObjectPool;
    private EcsPool<TransformComponent> _transformPool;
    private float _updatesPerSecond = 10;
    private float _currentUpdateTimer = 0f;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.UseObject, UseSelectedObject);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PickupComponent>().Exc<PooledObjectTag>().End();
        _pickupPool = world.GetPool<PickupComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        _usableObjectPool = world.GetPool<UsableObjectComponent>();

        EcsEventBus.Subscribe(GameplayEventType.UseObject, UseSelectedObject);
    }

    public void Run(IEcsSystems systems)
    {
        _currentUpdateTimer -= Time.deltaTime;
        if(_currentUpdateTimer > 0) return;
        //system logic
        foreach (int entity in _filter)
        {
            ref var transformComponent = ref _transformPool.Get(entity);
            ref var pickupComponent = ref _pickupPool.Get(entity);
            //lf items, sort by distance
            Vector3 senderPos = transformComponent.Transform.position;
            var objectsInRadiusSorted = 
            PhysicsHelper.GetAllEntitiesInRadius(entity, transformComponent.Transform.position, pickupComponent.PickupRadius)
            .Where(a => _usableObjectPool.Has(a))
            .Where(a => _usableObjectPool.Get(a).IsSelectable)
            .OrderBy(a => (senderPos - _transformPool.Get(a).Transform.position).magnitude)
            .ToList();
            if(objectsInRadiusSorted.Count == 0 && pickupComponent.UsableObjectIsSelected)
            {
                ref var currentItemUsableObjectComp = ref _usableObjectPool.Get(pickupComponent.CurrentSelectedUsableObject);
                if(currentItemUsableObjectComp.ActionOnDeselect != null) currentItemUsableObjectComp.ActionOnDeselect.Action(pickupComponent.CurrentSelectedUsableObject, null);
                pickupComponent.UsableObjectIsSelected = false;
                continue;
            }
            if(objectsInRadiusSorted.Count == 0) continue;
            if(pickupComponent.UsableObjectIsSelected)
            {
                if(pickupComponent.CurrentSelectedUsableObject == objectsInRadiusSorted.First()) { continue; }
                //olt item deselect actions
                ref var oldItemUsableObjectComp = ref _usableObjectPool.Get(pickupComponent.CurrentSelectedUsableObject);
                if(oldItemUsableObjectComp.ActionOnDeselect != null) oldItemUsableObjectComp.ActionOnDeselect.Action(pickupComponent.CurrentSelectedUsableObject, null);
                //new item select actions
                pickupComponent.CurrentSelectedUsableObject = objectsInRadiusSorted.First();
                ref var newItemUsableObjectComp = ref _usableObjectPool.Get(pickupComponent.CurrentSelectedUsableObject);
                if(newItemUsableObjectComp.ActionOnSelect != null) newItemUsableObjectComp.ActionOnSelect.Action(pickupComponent.CurrentSelectedUsableObject, null);
            }
            else
            {
                pickupComponent.CurrentSelectedUsableObject = objectsInRadiusSorted.First();
                ref var currentItemUsableObjectComp = ref _usableObjectPool.Get(pickupComponent.CurrentSelectedUsableObject);
                if(currentItemUsableObjectComp.ActionOnSelect != null) currentItemUsableObjectComp.ActionOnSelect.Action(pickupComponent.CurrentSelectedUsableObject, null);
                pickupComponent.UsableObjectIsSelected = true;
            }
        }
        //reset timer
        _currentUpdateTimer = 1f / _updatesPerSecond;
    }

    private void UseSelectedObject(int sender, EventArgs args)
    {
        ref var pickupComponent = ref _pickupPool.Get(sender);
        if(!pickupComponent.UsableObjectIsSelected) return;
        ref var usableObjectComponent = ref _usableObjectPool.Get(pickupComponent.CurrentSelectedUsableObject);
        if(usableObjectComponent.ActionOnUse != null) usableObjectComponent.ActionOnUse.Action(pickupComponent.CurrentSelectedUsableObject, null);
    }
}
