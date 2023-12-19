using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Voody.UniLeo.Lite;

public class ObjectPoolSystem : IEcsInitSystem, IEcsDestroySystem
{
    private List<ObjectPoolContainer> _poolContainer;

    private EcsPool<ObjectPoolHandlerComponent> _objectPoolHandlerPool;

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
        EcsEventBus.Subscribe(GameplayEventType.ReturnObjectToPool, ReturnObjectToPool);
        EcsEventBus.Subscribe(GameplayEventType.TakeObjectFromPool, TakeObjectFromPool);
    }

    private void ReturnObjectToPool(int entity, EventArgs args)
    {
        var returnToPoolArgs = args as ReturnObjectToPoolEventArgs;
        ref var poolHandlerComp = ref _objectPoolHandlerPool.Get(entity);
        foreach (var action in poolHandlerComp.ActionsOnReturnToPool)
        {
            action.Action(entity, null);
        }
        var container = _poolContainer.FirstOrDefault(a => a.PoolId == returnToPoolArgs.PoolId);
        container.ReturnObjectToPool(poolHandlerComp.GameObjectReference);
    }

    private void TakeFloatingTextFromPool(int entity, EventArgs args)
    {
        
    }

    private void TakeObjectFromPool(int entity, EventArgs args)
    {
        var takeObjectArgs = args as TakeObjectFromPoolEventArgs;
        var container = _poolContainer.FirstOrDefault(a => a.PoolId == takeObjectArgs.PoolId);
        if(container == null)
        {
            container = new ObjectPoolContainer(takeObjectArgs.PoolId, takeObjectArgs.ObjectToSpawn);
            _poolContainer.Add(container);
        }
        var takenObject = container.TakeObjectFromPool(takeObjectArgs.Position, takeObjectArgs.Rotation, _poolParent.transform);

        foreach(IObjectPoolStatsRestore resetAction in takenObject.GetComponent<ObjectPoolHandler>().ComponentResetList)
        {
            resetAction.ResetStats(takenObject);
        }
        if(takenObject.TryGetComponent<SummonedObject>(out var summ)) summ.SetMasterEntity(entity);

    }
}

public class ObjectPoolContainer
{
    private int _poolId;

    private List<GameObject> _disabledObjects;

    private List<GameObject> _activeObjects;

    private GameObject _prefab;

    public int PoolId => _poolId;

    public ObjectPoolContainer(int poolId, GameObject prefab)
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
            world.GetPool<PooledObjectTag>().Del(entity);
            oldObject.SetActive(true);
            return oldObject;
        }
    }

    public void ReturnObjectToPool(GameObject gameObj)
    {
        _disabledObjects.Add(gameObj);
        _activeObjects.Remove(gameObj);

        int entity = gameObj.GetComponent<ConvertToEntity>().TryGetEntity().Value;
        var world = EcsStart.World;
        world.GetPool<PooledObjectTag>().Add(entity);
        gameObj.SetActive(false);

    }
}
