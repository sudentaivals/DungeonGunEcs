using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Unparent gun action")]
public class UnparentGunAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        EcsEventBus.Publish(GameplayEventType.UnparentGun, senderEntity, null);
    }
}
