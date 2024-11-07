using System;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class HealthSystem : IEcsInitSystem, IEcsDestroySystem
{
    private EcsPool<HealthComponent> _healthPool;
    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.TakeDamage, TakeDamage);
        EcsEventBus.Unsubscribe(GameplayEventType.OnEntityDamageTaken, OnEntityDamageTake);
        EcsEventBus.Unsubscribe(GameplayEventType.AddNewOnDamageTakeAction, AddNewOnDamageTakeEvent);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _healthPool = world.GetPool<HealthComponent>();
        EcsEventBus.Subscribe(GameplayEventType.TakeDamage, TakeDamage);
        EcsEventBus.Subscribe(GameplayEventType.OnEntityDamageTaken, OnEntityDamageTake);
        EcsEventBus.Subscribe(GameplayEventType.AddNewOnDamageTakeAction, AddNewOnDamageTakeEvent);
    }

    private void TakeDamage(int damageTakerEntity, EventArgs args)
    {
        var takeDamageArgs = args as TakeDamageEventArgs;
        var damageInformation = takeDamageArgs.DamageInformation;
        var damageDealerEntity = takeDamageArgs.DamageDealerEntity;
        ref var damageTakerHealthComponent = ref _healthPool.Get(damageTakerEntity);
        if(!damageTakerHealthComponent.IsAlive) return;

        //evasion
        var randomNumber = UnityEngine.Random.Range(0f, 1f);
        if(randomNumber < damageTakerHealthComponent.GetClampedEvasion)
        {
            damageTakerHealthComponent.OnEvasion.ForEach((evasionAction) =>
            {
                if(evasionAction == null) return;
                evasionAction.Action(damageTakerEntity, damageDealerEntity);
            });
            return;
        }

        //damage modificators
        var damageModArgs = ConditionAndActionArgsPool.GetArgs<DamageArgs>();
        damageModArgs.DamageInformation = damageInformation;
        foreach (var damageModificator in damageTakerHealthComponent.IncomingDamageModificators)
        {
            if(damageModificator == null) continue;
            damageModificator.ModifyDamage(damageTakerEntity, damageDealerEntity, damageModArgs);
        }
        //damage & flat damage calc
        float damage = damageInformation.InitialDamage + damageInformation.FlatBonusDamage;

        //damage % mod
        float damageModificatorPercent = damageInformation.DamageMultiplier + 1f;
        damageModificatorPercent = Mathf.Clamp(damageModificatorPercent, 0f, float.MaxValue);
        damage *= damageModificatorPercent;

        //critical hit calculations
        if(takeDamageArgs.DamageInformation.IsCriticalHit)
        {
            float criticalMultiplier = damageInformation.CriticalHitMultiplierBonus + Settings.MinimumCriticalMultiplier;
            criticalMultiplier = Mathf.Clamp(criticalMultiplier, Settings.MinimumCriticalMultiplier, float.MaxValue);
            damage *= criticalMultiplier;
        }

        //damage multiplier
        float damageMultiplicator = damageInformation.DamageMultiplicativeBonus.Aggregate(1.0f, (x, y) => x * y);

        //round damage
        int damageInt = Mathf.RoundToInt(damage);
        damageInformation.FinalDamage = damageInt;
        Debug.Log(damageInformation.FinalDamage);

        //deal damage
        damageTakerHealthComponent.CurrentHealth -= damageInt;
        var onEntityDamageDealArgs = EventArgsObjectPool.GetArgs<OnDamageDealEventArgs>();
        onEntityDamageDealArgs.DamageTakerEntity = damageTakerEntity;
        onEntityDamageDealArgs.DamageInformation = damageInformation;
        EcsEventBus.Publish(GameplayEventType.OnEntityDamageDeal, damageDealerEntity, onEntityDamageDealArgs);

        //decrease health
        if(damageTakerHealthComponent.IsAlive == false)
        {
            foreach (var action in damageTakerHealthComponent.OnDeath)
            {
                if(action == null) continue;
                action.Action(damageTakerEntity, damageDealerEntity);
            }
        }
    }

    private void OnEntityDamageTake(int damageTakerEntity, EventArgs args)
    {
        if(!_healthPool.Has(damageTakerEntity)) return;
        var onDamageTakeArgs = args as OnDamageTakeEventArgs;

        ref var damageTakerHealthComponent = ref _healthPool.Get(damageTakerEntity);
        var damageAndConditionArg = ConditionAndActionArgsPool.GetArgs<DamageArgs>();
        damageAndConditionArg.DamageInformation = onDamageTakeArgs.DamageInformation;
        foreach (var damageAction in damageTakerHealthComponent.ActionsOnDamageTake)
        {
            if(damageAction == null) continue;
            damageAction.Action(damageTakerEntity, onDamageTakeArgs.DamageDealerEntity, damageAndConditionArg);
        }
    }

    private void AddNewOnDamageTakeEvent(int damageTakerEntity, EventArgs args)
    {
        if(!_healthPool.Has(damageTakerEntity)) return;
        var addNewOnDamageTakeEventArgs = args as AddOrRemoveOnDamageTakeActionEventArgs;
        ref var damageTakerHealthComponent = ref _healthPool.Get(damageTakerEntity);
        damageTakerHealthComponent.ActionsOnDamageTake.Add(addNewOnDamageTakeEventArgs.NewOnDamageTakeAction);
    }

    private void RemoveOnDamageTakeEvent(int damageTakerEntity, EventArgs args)
    {
        if(!_healthPool.Has(damageTakerEntity)) return;
        var addOrRemoveOnDamageTakeEventArgs = args as AddOrRemoveOnDamageTakeActionEventArgs;
        ref var damageTakerHealthComponent = ref _healthPool.Get(damageTakerEntity);
        if(damageTakerHealthComponent.ActionsOnDamageTake.Contains(addOrRemoveOnDamageTakeEventArgs.NewOnDamageTakeAction))
        {
            damageTakerHealthComponent.ActionsOnDamageTake.Remove(addOrRemoveOnDamageTakeEventArgs.NewOnDamageTakeAction);
        }

    }
}
