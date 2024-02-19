using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Object pool reset/Reset corpse component")]
public class ResetCorpseComponent : ScriptableObject, IObjectPoolStatsRestore
{
    public void ResetStats(GameObject gameObject)
    {
        if(gameObject.TryGetComponent<CorpseMbHelper>(out var corpseMbHelper))
        {
            corpseMbHelper.CorpseDataIsTransfered = false;
        }
    }
}
