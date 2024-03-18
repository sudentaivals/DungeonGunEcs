using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Repeat game action")]
public class RepeatAction : GameAction
{
    [SerializeField] int _repeatCount;
    [SerializeField] GameAction _action;

    public override void Action(int senderEntity, int? takerEntity)
    {
        for (int i = 0; i < _repeatCount; i++)
        {
            _action.Action(senderEntity, takerEntity);
        }
    }
}
