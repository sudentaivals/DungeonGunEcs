using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/DamageMod/Conditions/Critical hit condition")]
public class DamageCriticalHitCondition : BaseGameCondition
{
    [SerializeField] private bool _isCriticalHit;
    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        if(conditionArgs == null)
        {
            throw new System.Exception("ConditionAndActionArgs is null");
        }
        if (conditionArgs is not DamageArgs args) return false;
        return _isCriticalHit ? args.DamageInformation.IsCriticalHit == true : args.DamageInformation.IsCriticalHit == false;
    
    }
}
