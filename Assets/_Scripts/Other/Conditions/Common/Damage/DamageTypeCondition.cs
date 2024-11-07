using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/DamageMod/Conditions/Damage type condition")]
public class DamageTypeCondition : BaseGameCondition
{
    [SerializeField] private DamageType _damageType;
    [SerializeField] private bool _reverseCheck = false;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        if(conditionArgs == null)
        {
            throw new System.Exception("ConditionAndActionArgs is null");
        }
        if (conditionArgs is not DamageArgs args) return false;
        
        if (_reverseCheck)
        {
            return args.DamageInformation.DamageType != _damageType;
        }
        return args.DamageInformation.DamageType == _damageType;
    }
}
