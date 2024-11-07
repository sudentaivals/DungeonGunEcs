using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/New game action")]
public class StartRollAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        return;
    }
}
