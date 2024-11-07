using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/DamageMod/Conditions/Damage id condition")]
public class DamageIdCondition : BaseGameCondition
{
    [SerializeField] private DamageSO _damageSo;
    [SerializeField] private bool _reverseCheck = false;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        if(conditionArgs == null)
        {
            throw new System.Exception("ConditionAndActionArgs is null");
        }
        if (conditionArgs is not DamageArgs args) return false;
        return _reverseCheck ? args.DamageInformation.DamageId != _damageSo.Guid : args.DamageInformation.DamageId == _damageSo.Guid;
    }
}
