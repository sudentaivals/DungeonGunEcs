using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset bullet")]
public class ResetBulletComponent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        var entity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity();
        if (entity == null) return;
        var world = EcsStart.World;
        var pool = world.GetPool<BulletStatsComponent>();
        ref var bulletStats = ref pool.Get(entity.Value);
        bulletStats.CurrentFlyDistance = 0;
        bulletStats.IsActivated = false;
    }
}
