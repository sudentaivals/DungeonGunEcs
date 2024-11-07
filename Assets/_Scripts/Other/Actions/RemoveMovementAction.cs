using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Remove movement action")]
public class RemoveMovementAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        EcsEventBus.Publish(GameplayEventType.RemoveMovement, senderEntity, null);
    }
}
