using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Voody.UniLeo.Lite;

public class ObjectPoolSystem : IEcsInitSystem, IEcsDestroySystem
{
    private List<ObjectPoolContainer> _poolContainer;

    private EcsPool<ObjectPoolHandlerComponent> _objectPoolHandlerPool;

    private EcsPool<NpcIdComponent> _npcIdPool;

    private EcsPool<EventsComponent> _eventsComponentPool;
    private GameObject _poolParent;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ReturnObjectToPool, ReturnObjectToPool);
        EcsEventBus.Unsubscribe(GameplayEventType.TakeObjectFromPool, TakeObjectFromPool);
    }

    public void Init(IEcsSystems systems)
    {
        _poolParent = new GameObject("ObjectPool");
        _poolParent.transform.position = Vector3.zero;
        var world = systems.GetWorld();
        _poolContainer = new();
        _objectPoolHandlerPool = world.GetPool<ObjectPoolHandlerComponent>();
        _npcIdPool = world.GetPool<NpcIdComponent>();
        _eventsComponentPool = world.GetPool<EventsComponent>();
        EcsEventBus.Subscribe(GameplayEventType.ReturnObjectToPool, ReturnObjectToPool);
        EcsEventBus.Subscribe(GameplayEventType.TakeObjectFromPool, TakeObjectFromPool);
    }

    private void ReturnObjectToPool(int entity, EventArgs args)
    {
        ref var poolHandlerComp = ref _objectPoolHandlerPool.Get(entity);
        foreach (var action in poolHandlerComp.ActionsOnReturnToPool)
        {
            action.Action(entity, null);
        }
        ref var npcIdComponent = ref _npcIdPool.Get(entity);
        Guid poolId = npcIdComponent.SerializableGuid.Guid;
        var container = GetContainer(poolId, poolHandlerComp.GameObjectReference);
        container.ReturnObjectToPool(poolHandlerComp.GameObjectReference);
    }

    private ObjectPoolContainer GetContainer(Guid poolId, GameObject prefab)
    {
        var container = _poolContainer.FirstOrDefault(a => a.PoolId == poolId);
        if(container == null)
        {
            container = new ObjectPoolContainer(poolId, prefab);
            _poolContainer.Add(container);
        }
        return container;
    }

    private void TakeObjectFromPool(int entity, EventArgs args)
    {
        var takeObjectArgs = args as TakeObjectFromPoolEventArgs;
        if(takeObjectArgs.ObjectToSpawn == null) return;
        Guid poolId = Guid.Empty;
        if(takeObjectArgs.ObjectToSpawn.TryGetComponent<ObjectPoolHandler>(out var poolHandler))
        {
            poolId = poolHandler.Guid;
        }
        else
        {
            return;
        }
        var container = GetContainer(poolId, takeObjectArgs.ObjectToSpawn);
        var takenObject = container.TakeObjectFromPool(takeObjectArgs.Position, takeObjectArgs.Rotation, _poolParent.transform);

        int? pooledObjectEntity = takenObject.GetComponent<ConvertToEntity>().TryGetEntity();

        if(pooledObjectEntity != null)
        {
            ref var poolHandlerComp = ref _objectPoolHandlerPool.Get(pooledObjectEntity.Value);
            foreach (IObjectPoolStatsRestore action in poolHandlerComp.ComponentsResetList)
            {

               action.ResetStats(takenObject);
            }
        }

        //start events reseted, no matter what
        if(pooledObjectEntity != null)
        {
            if(_eventsComponentPool.Has(pooledObjectEntity.Value))
            {
                ref var eventsComp = ref _eventsComponentPool.Get(pooledObjectEntity.Value);
                eventsComp.StartEventsInvoked = false;
            }
        }

        if(takeObjectArgs.StatsSetters == null) return;
        foreach (var action in takeObjectArgs.StatsSetters)
        {
            action.SetStats(takenObject, entity);
        }
    }
}

public class ObjectPoolContainer
{
    private Guid _poolId;

    private List<GameObject> _disabledObjects;

    private List<GameObject> _activeObjects;

    private GameObject _prefab;

    public Guid PoolId => _poolId;

    public ObjectPoolContainer(Guid poolId, GameObject prefab)
    {
        _disabledObjects = new();
        _activeObjects = new();
        _prefab = prefab;
        _poolId = poolId;
    }

    public GameObject TakeObjectFromPool(Vector3 position, Quaternion rotation, Transform parent)
    {
        if(_disabledObjects.Count == 0)
        {
            var newObject = GameObject.Instantiate(_prefab, position, rotation, parent);
            _activeObjects.Add(newObject);
            return newObject;
        }
        else
        {
            var oldObject = _disabledObjects.First();
            _disabledObjects.Remove(oldObject);
            _activeObjects.Add(oldObject);
            oldObject.transform.position = position;
            oldObject.transform.rotation = rotation;

            int entity = oldObject.GetComponent<ConvertToEntity>().TryGetEntity().Value;
            var world = EcsStart.World;
            var pool = world.GetPool<PooledObjectTag>();
            if(pool.Has(entity)) pool.Del(entity);
            oldObject.SetActive(true);
            return oldObject;
        }
    }

    public void ReturnObjectToPool(GameObject gameObj)
    {
        if(_disabledObjects.Contains(gameObj)) return;
        _disabledObjects.Add(gameObj);
        _activeObjects.Remove(gameObj);

        int entity = gameObj.GetComponent<ConvertToEntity>().TryGetEntity().Value;
        var world = EcsStart.World;
        var pool = world.GetPool<PooledObjectTag>();
        if(!pool.Has(entity)) pool.Add(entity);
        gameObj.SetActive(false);

    }
}
