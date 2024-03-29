using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Create action with probability")]
public class ActionWithProbability : GameAction
{
    [Range(0f, 1f)]
    [SerializeField] private float _probability;
    [SerializeField] private GameAction _action;
    public override void Action(int senderEntity, int? takerEntity)
    {
        if (Random.Range(0f, 1f) < _probability) _action.Action(senderEntity, takerEntity);
    }
}