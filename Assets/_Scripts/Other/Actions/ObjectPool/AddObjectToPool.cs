using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add object to pool")]
public class AddObjectToPool : GameAction
{
    public override void Action(int senderEntity, int? takerEntity)
    {
        EcsEventBus.Publish(GameplayEventType.ReturnObjectToPool, senderEntity, null);
    }
}
