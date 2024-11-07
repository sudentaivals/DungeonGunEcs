using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Take corpse from object pool")]
public class TakeCorpseFromObjectPool : GameAction
{
    [SerializeField] GameObject _corpsePrefab;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        EcsEventBus.Publish(GameplayEventType.TakeCorpseFromPool, senderEntity, null);
    }
}
