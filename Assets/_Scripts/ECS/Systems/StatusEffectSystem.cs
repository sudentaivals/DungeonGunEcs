using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<StatusEffectsComponent> _statusEffectPool;
    private EcsPool<SummonedObjectComponent> _summonedObjectPool;

    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _statusEffectPool = null;
        _summonedObjectPool = null;
        EcsEventBus.Unsubscribe(GameplayEventType.AddStatusEffect, AddStatusEffect);
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveAllStatusEffects, RemoveAllStatusEffects);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<StatusEffectsComponent>().Exc<PooledObjectTag>().End();
        _statusEffectPool = world.GetPool<StatusEffectsComponent>();
        _summonedObjectPool = world.GetPool<SummonedObjectComponent>();
        EcsEventBus.Subscribe(GameplayEventType.AddStatusEffect, AddStatusEffect);
        EcsEventBus.Subscribe(GameplayEventType.RemoveAllStatusEffects, RemoveAllStatusEffects);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var statusEffectComp = ref _statusEffectPool.Get(entity);
            if(statusEffectComp.StatusEffects == null)
            {
                statusEffectComp.StatusEffects = new List<StatusEffect>();
            }
            foreach (var se in statusEffectComp.StatusEffects)
            {
                se.UpdateStatusEffect(Time.deltaTime);
            }
            statusEffectComp.StatusEffects.RemoveAll(a => a.IsOver);
        }
    }

    private void RemoveAllStatusEffects(int senderEntity, EventArgs args)
    {
        ref var statusEffectComp = ref _statusEffectPool.Get(senderEntity);
        foreach (var se in statusEffectComp.StatusEffects)
        {
            se.OnRemove();
        }
        statusEffectComp.StatusEffects.Clear();
    }

    private void AddStatusEffect(int senderEntity, EventArgs args)
    {
        var statusEffectsArgs = args as AddStatusEffectEventArgs;
        if (!_statusEffectPool.Has(statusEffectsArgs.TargetEntity))
        {
            return;
        }
        ref var statusEffectComponent = ref _statusEffectPool.Get(statusEffectsArgs.TargetEntity);
        //set sender entity
        int sennder = 0;
        if (_summonedObjectPool.Has(senderEntity))
        {
            ref var summonedObjComp = ref  _summonedObjectPool.Get(senderEntity);
            sennder = summonedObjComp.MasterId;
        }
        else
        {
            sennder = senderEntity;
        }
        //check for skill apply type
        var statusEffect = new StatusEffect(statusEffectsArgs.EffectShell, statusEffectsArgs.TargetEntity, sennder);
        switch (statusEffectsArgs.EffectShell.SkillApplyType)
        {
            case SkillApplyType.UniqueForAll:
                if (statusEffectComponent.StatusEffects.Any(statusEffect => statusEffect.Id == statusEffectsArgs.EffectShell.Id)) break;
                statusEffectComponent.StatusEffects.Add(statusEffect);
                break;
            case SkillApplyType.UniquePerObject:
                if (statusEffectComponent.StatusEffects.Any(a => a.Id == statusEffectsArgs.EffectShell.Id && a.SenderEntity == sennder)) break;
                statusEffectComponent.StatusEffects.Add(statusEffect);
                break;
            case SkillApplyType.Unlimited:
                statusEffectComponent.StatusEffects.Add(statusEffect);
                break;
            default:
                break;
        }
    }
}
