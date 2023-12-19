using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class UiHealthbarSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;

    private EcsPool<HealthComponent> _healthPool;

    private EcsPool<UiHealthComponent> _uiHealthPool;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<HealthComponent>().Inc<UiHealthComponent>().End();
        _healthPool = world.GetPool<HealthComponent>();
        _uiHealthPool = world.GetPool<UiHealthComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var healthComponent = ref _healthPool.Get(entity);
            ref var uiHealthComponent = ref _uiHealthPool.Get(entity);

            uiHealthComponent.HealthBar.fillAmount = (float)healthComponent.CurrentHealth / (healthComponent.MaxHealth + healthComponent.MaxHealthBonus);
        }
    }
}
