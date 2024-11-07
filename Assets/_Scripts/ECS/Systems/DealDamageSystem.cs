using Leopotam.EcsLite;
using System;
using System.Linq;

public class DealDamageSystem : IEcsInitSystem, IEcsDestroySystem
{
    private EcsPool<DamageDealComponent> _damageDealPool;
    private EcsPool<SummonedObjectComponent> _summonedObjectPool;
    private DamageInformation _damageInformation;
    private void DealDamage(int damageDealerEntity, EventArgs args)
    {
        var damageArgs = args as DealDamageEventArgs;
        ref var damageDealComponent = ref _damageDealPool.Get(damageDealerEntity);
        //check for summoned object
        if(_summonedObjectPool.Has(damageDealerEntity))
        {
            ref var damageDealerSummonedObjectComponent = ref _summonedObjectPool.Get(damageDealerEntity);
            damageDealComponent = ref _damageDealPool.Get(damageDealerSummonedObjectComponent.MasterId);
        }
        _damageInformation.UpdateVariables(damageArgs.InitialDamage, damageArgs.DamageId, damageArgs.DamageType);
        //damage modifications
        var damageModArgs = ConditionAndActionArgsPool.GetArgs<DamageArgs>();
        damageModArgs.DamageInformation = _damageInformation;
        foreach (var damageMod in damageDealComponent.DamageModificators)
        {
            if(damageMod == null) continue;
            damageMod.ModifyDamage(damageArgs.DamageTakerEntity, damageDealerEntity, damageModArgs);
        }
        //critical hit calculation
        var randomNumber = UnityEngine.Random.Range(0f, 1f);
        var criticalHitChance = _damageInformation.CriticalHitBonus;
        bool isCriticalHit = randomNumber < criticalHitChance;
        if(isCriticalHit) _damageInformation.IsCriticalHit = true;

        //send take damage event
        var takeDamageEventArgs = EventArgsObjectPool.GetArgs<TakeDamageEventArgs>();
        takeDamageEventArgs.DamageInformation = _damageInformation;
        takeDamageEventArgs.DamageDealerEntity = damageDealerEntity;

        EcsEventBus.Publish(GameplayEventType.TakeDamage, damageArgs.DamageTakerEntity, takeDamageEventArgs);
    }

    private void AddDamageModificator(int senderEntity, EventArgs args)
    {
        var damageModArgs = args as AddOrRemoveDamageModificatorEventArgs;
        if(!_damageDealPool.Has(senderEntity)) return;
        ref var damageDealComponent = ref _damageDealPool.Get(senderEntity);
        damageDealComponent.DamageModificators.Add(damageModArgs.DamageModificatorContainer);
    }

    private void AddNewOnDamageEvent(int senderEntity, EventArgs args)
    {
        if(!_damageDealPool.Has(senderEntity)) return;
        var onDamageDealArgs = args as AddOrRemoveOnDamageDealActionEventArgs;
        ref var damageDealComponent = ref _damageDealPool.Get(senderEntity);
        damageDealComponent.ActionsOnDamageDeal.Add(onDamageDealArgs.DamageAction);
    }

    private void RemoveDamageModificator(int senderEntity, EventArgs args)
    {
        if(!_damageDealPool.Has(senderEntity)) return;
        var damageModArgs = args as AddOrRemoveDamageModificatorEventArgs;
        ref var damageDealComponent = ref _damageDealPool.Get(senderEntity);
        if(damageModArgs.DamageModificatorContainer == null) return;
        if(damageDealComponent.DamageModificators.Contains(damageModArgs.DamageModificatorContainer))
        {
            damageDealComponent.DamageModificators.Remove(damageModArgs.DamageModificatorContainer);
        }
    }

    private void RemoveOnDamageEvent(int senderEntity, EventArgs args)
    {
        if(!_damageDealPool.Has(senderEntity)) return;
        var onDamageDealArgs = args as AddOrRemoveOnDamageDealActionEventArgs;
        ref var damageDealComponent = ref _damageDealPool.Get(senderEntity);
        if(damageDealComponent.ActionsOnDamageDeal.Contains(onDamageDealArgs.DamageAction))
        {
            damageDealComponent.ActionsOnDamageDeal.Remove(onDamageDealArgs.DamageAction);
        }
    }

    private void OnEntityDamageDeal(int senderEntity, EventArgs args)
    {
        var onDamageDealArgs = args as OnDamageDealEventArgs;
        if(!_damageDealPool.Has(senderEntity)) return;
        ref var damageDealComponent = ref _damageDealPool.Get(senderEntity);
        var damageAndConditionArg = ConditionAndActionArgsPool.GetArgs<DamageArgs>();
        damageAndConditionArg.DamageInformation = onDamageDealArgs.DamageInformation;
        damageDealComponent.ActionsOnDamageDeal.ForEach((damageDealAction) =>
        {
            if(damageDealAction == null) return;
            damageDealAction.Action(senderEntity, onDamageDealArgs.DamageTakerEntity, damageAndConditionArg);
        });
    }

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.DealDamage, DealDamage);
        EcsEventBus.Unsubscribe(GameplayEventType.AddNewDamageModificator, AddDamageModificator);
        EcsEventBus.Unsubscribe(GameplayEventType.OnEntityDamageDeal, OnEntityDamageDeal);
        EcsEventBus.Unsubscribe(GameplayEventType.AddNewOnDamageDealAction, AddNewOnDamageEvent);
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveOnDamageEvent, RemoveOnDamageEvent);
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveDamageModificator, RemoveDamageModificator);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _damageDealPool = world.GetPool<DamageDealComponent>();
        _summonedObjectPool = world.GetPool<SummonedObjectComponent>();
        _damageInformation = new();

        EcsEventBus.Subscribe(GameplayEventType.DealDamage, DealDamage);
        EcsEventBus.Subscribe(GameplayEventType.AddNewDamageModificator, AddDamageModificator);
        EcsEventBus.Subscribe(GameplayEventType.OnEntityDamageDeal, OnEntityDamageDeal);
        EcsEventBus.Subscribe(GameplayEventType.AddNewOnDamageDealAction, AddNewOnDamageEvent);
        EcsEventBus.Subscribe(GameplayEventType.RemoveOnDamageEvent, RemoveOnDamageEvent);
        EcsEventBus.Subscribe(GameplayEventType.RemoveDamageModificator, RemoveDamageModificator);
    }
}
