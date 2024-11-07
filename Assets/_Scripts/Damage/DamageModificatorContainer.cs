using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageModContainer", menuName = "My Assets/DamageMod/DamageModContainer")]
public class DamageModificatorContainer : ScriptableObject
{
    [Header("Filters")]
    [Tooltip("Filters will check damage deal conditions on entity level. If any filter returns false, damage will not be modified.")]
    [SerializeField] private List<BaseGameCondition> _filters = new();
    [Header("Modificator")]
    [SerializeField] private DamageModificator _damageModificator;
    [Header("Additional information")]
    [TextArea(10, 15)]
    [SerializeField] private string _damageNote;


    public bool ModifyDamage(int damageTakerEntity, int damageSenderEntity, ConditionAndActionArgs conditionAndActionArgs)
    {
        foreach (var filter in _filters)
        {
            if(!filter.CheckCondition(damageSenderEntity, damageTakerEntity, conditionAndActionArgs)) return false;
        }
        var args = conditionAndActionArgs as DamageArgs;
        var damageInformation =  args.DamageInformation;
        _damageModificator.ModifyDamage(ref damageInformation);
        return true; 
    }
}
