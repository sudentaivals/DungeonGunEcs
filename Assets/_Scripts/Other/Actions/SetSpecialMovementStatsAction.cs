using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Set special movement stats")]
public class SetSpecialMovementStatsAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        EcsEventBus.Publish(GameplayEventType.ActivateSpecialMovement, senderEntity, null);
    }
}
