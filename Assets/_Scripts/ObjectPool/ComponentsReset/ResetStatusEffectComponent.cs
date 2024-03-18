using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset remove all status effects")]
public class ResetStatusEffectComponent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        var entity = gameObject.GetComponent<ConvertToEntity>().TryGetEntity();
        if (entity == null) return;
        EcsEventBus.Publish(GameplayEventType.RemoveAllStatusEffects, entity.Value, null);
    }
}
