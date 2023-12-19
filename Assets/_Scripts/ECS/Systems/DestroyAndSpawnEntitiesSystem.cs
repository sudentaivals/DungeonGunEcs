using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAndSpawnEntitiesSystem : IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem
{
    private Queue<EntityDestroyContainer> _destroyQueue;
    private Queue<EntitySpawnContainer> _spawnQueue;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<EventsComponent> _eventsPool;
    private void KillEntity(int sender, EventArgs args)
    {
        var gameObject = _transformPool.Get(sender).Transform.gameObject;
        _destroyQueue.Enqueue(new EntityDestroyContainer(gameObject, sender));
    }

    private void SpawnEntity(int sender, EventArgs args)
    {
        var spawnArgs = args as SpawnEntityEventArgs;
        _spawnQueue.Enqueue(new EntitySpawnContainer(spawnArgs.PrefabToSpawn, spawnArgs.Position, spawnArgs.Rotation, sender));
    }

    public void Init(IEcsSystems systems)
    {
        var ecsWorld = systems.GetWorld();
        _transformPool = ecsWorld.GetPool<TransformComponent>();
        _eventsPool = ecsWorld.GetPool<EventsComponent>();

        _destroyQueue = new Queue<EntityDestroyContainer>();
        _spawnQueue = new Queue<EntitySpawnContainer>();

        EcsEventBus.Subscribe(GameplayEventType.DestroyObject, KillEntity);
        EcsEventBus.Subscribe(GameplayEventType.SpawnObject, SpawnEntity);

    }

    public void Destroy(IEcsSystems systems)
    {
        _destroyQueue = null;
        _spawnQueue = null;
        _transformPool = null;
        _eventsPool = null;

        EcsEventBus.Unsubscribe(GameplayEventType.DestroyObject, KillEntity);
        EcsEventBus.Unsubscribe(GameplayEventType.SpawnObject, SpawnEntity);

    }

    public void Run(IEcsSystems systems)
    {                
        //spawn new entities
        while (_spawnQueue.TryDequeue(out var result))
        {
            var spawned = GameObject.Instantiate(result.GameObject, result.Position, result.Rotation);
            if (spawned.TryGetComponent<SummonedObject>(out var summoned))
            {
                summoned.SetMasterEntity(result.SpawnerEntity);
            }
        }
        //clear entities list
        while (_destroyQueue.TryDequeue(out var result))
        {
            ref var entityEvents = ref _eventsPool.Get(result.Entity);
            foreach (var deathEvent in entityEvents.OnDeath)
            {
                deathEvent?.Action(result.Entity, null);
            }
            EcsStart.World.DelEntity(result.Entity);
            GameObject.Destroy(result.GameObject);
        }
    }
}

public struct EntityDestroyContainer
{
    public GameObject GameObject { get; set; }

    public int Entity { get; set; }

    public EntityDestroyContainer(GameObject gameObject, int entity)
    {
        GameObject = gameObject;
        Entity = entity;
    }

}

public struct EntitySpawnContainer
{
    public GameObject GameObject { get; set; }

    public Vector3 Position { get; set; }

    public Quaternion Rotation { get; set; }
    public int SpawnerEntity { get; set; }

    public EntitySpawnContainer(GameObject gameObject, Vector3 position, Quaternion rotation, int spawnerEntity)
    {
        GameObject = gameObject;
        Position = position;
        Rotation = rotation;
        SpawnerEntity = spawnerEntity;
    }
}
