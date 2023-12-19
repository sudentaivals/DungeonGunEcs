using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset collision event")]
public class ResetCollisionEvent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        if(gameObject.TryGetComponent<CollisionHandler>(out var handler))
        {
            handler.ResetTrigger();
        }
    }
}

