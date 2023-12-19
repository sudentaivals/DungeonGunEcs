using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset master entity")]
public class ResetMasterEntity : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        if(gameObject.TryGetComponent<SummonedObject>(out var summoned))
        {
            summoned.SetMasterEntityState(false);
        }
    }
}

