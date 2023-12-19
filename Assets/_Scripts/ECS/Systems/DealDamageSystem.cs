using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private Queue<DamageData> _damageQueue;
    private EcsPool<GlobalStatsComponent> _statsPool;
    private EcsPool<EventsComponent> _eventsPool;
    private EcsPool<HealthComponent> _healthPool;
    private void AddDamageToQueue(int senderEntity, EventArgs args)
    {
        var damageArgs = args as DealDamageEventArgs;
        if (!_healthPool.Has(damageArgs.DamageTakerEntity)) return;
        var newDamageData = new DamageData(damageArgs.SenderEntity, damageArgs.SenderStats, damageArgs.DamageTakerEntity, damageArgs.Damage);
        _damageQueue.Enqueue(newDamageData);
    }

    public void Destroy(IEcsSystems systems)
    {
        _damageQueue = null;
        _statsPool = null;
        _eventsPool = null;
        EcsEventBus.Unsubscribe(GameplayEventType.DealDamage, AddDamageToQueue);
    }

    public void Init(IEcsSystems systems)
    {
        _damageQueue = new Queue<DamageData>();
        var world = systems.GetWorld();
        _statsPool = world.GetPool<GlobalStatsComponent>();
        _eventsPool = world.GetPool<EventsComponent>();
        _healthPool = world.GetPool<HealthComponent>();

        EcsEventBus.Subscribe(GameplayEventType.DealDamage, AddDamageToQueue);
    }

    public void Run(IEcsSystems systems)
    {
        while (_damageQueue.TryDequeue(out var result))
        {
            //get stats and events comp
            ref var takerEvents = ref _eventsPool.Get(result.TakerEntity);
            ref var takerStats = ref _statsPool.Get(result.TakerEntity);
            ref var takerHealth = ref _healthPool.Get(result.TakerEntity);
            GlobalStatsComponent senderStats = new GlobalStatsComponent();
            if(result.SenderStats == null)
            {
                senderStats = _statsPool.Get(result.SenderEntity.Value);
            }
            else
            {
                senderStats = result.SenderStats.Value;
            }
            //evasion
            var randomNumber = UnityEngine.Random.Range(0f, 1f);
            if(randomNumber < takerStats.GetClampedEvasion)
            {
                //takerEvents.OnEvasion?.Invoke();
                EcsEventBus.Publish(GameplayEventType.CreateFloatingText, result.TakerEntity, new CreateFloatingTextEventArgs("Evade", Color.white, new Vector2(1, -0.5f)));
                continue;
            }
            //hit - calculate damage
            var damage = (float)(result.Damage + senderStats.DamageBonusFlat) * (1f + senderStats.DamageBonusPercent);
            var damageAfterReduction = ((int)damage - takerStats.DamageReductionFlat) * (1 - takerStats.DamageReductionPercent);
            var clampedDamage = Mathf.RoundToInt(Mathf.Clamp(damageAfterReduction, 0f, float.MaxValue));
            EcsEventBus.Publish(GameplayEventType.CreateFloatingText, result.TakerEntity, new CreateFloatingTextEventArgs($"-{clampedDamage}", Color.cyan, new Vector2(1, 0.5f)));
            takerHealth.CurrentHealth -= Mathf.RoundToInt(clampedDamage);
        }
    }
}

public struct DamageData
{
    public int TakerEntity;
    public int Damage;
    public GlobalStatsComponent? SenderStats;
    public int? SenderEntity;
    public DamageData(int? senderEntity, GlobalStatsComponent? senderStats, int takerEntity, int damage)
    {
        SenderEntity = senderEntity;
        SenderStats = senderStats;
        TakerEntity = takerEntity;
        Damage = damage;
    }
}
