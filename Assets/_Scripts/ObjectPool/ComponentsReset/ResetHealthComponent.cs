using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset health")]
public class ResetHealthComponent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        var entity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity();
        if (entity == null) return;
        var world = EcsStart.World;
        var pool = world.GetPool<HealthComponent>();
        ref var healthComponent = ref pool.Get(entity.Value);
        healthComponent.CurrentHealth = healthComponent.MaxHealth + healthComponent.MaxHealthBonus;
    }
}
