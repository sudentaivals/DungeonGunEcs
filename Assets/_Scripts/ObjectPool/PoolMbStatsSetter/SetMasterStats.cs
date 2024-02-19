using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Mb stats setter/Set master stats")]
public class SetMasterStats : ScriptableObject, IMbHelperStatsSetter
{
    public void SetStats(GameObject takerGameObject, int senderEntity)
    {
        if(takerGameObject.TryGetComponent<SummonedObject>(out var summonedObject))
        {
            summonedObject.SetMasterEntity(senderEntity);
        }
    }
}
