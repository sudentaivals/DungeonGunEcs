using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset start events")]
public class ResetStartEvents : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        var entity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity();
        if (entity == null) return;
        var world = EcsStart.World;
        var pool = world.GetPool<EventsComponent>();
        if(!pool.Has(entity.Value)) return;
        ref var eventComponent = ref pool.Get(entity.Value);
        eventComponent.StartEventsInvoked = false;

    }
}
