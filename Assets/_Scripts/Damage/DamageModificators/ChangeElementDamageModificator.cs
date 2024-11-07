using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/DamageMod/Modificators/Change damage type mod")]
public class ChangeElementDamageModificator : DamageModificator
{
    [SerializeField] private DamageType _damageType;
    public override void ModifyDamage(ref DamageInformation damageInformation)
    {
        damageInformation.DamageType = _damageType;
    }
}
