using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/DamageMod/Modificators/Add flat damage bonus")]
public class FlatDamageModificator : DamageModificator
{
    [SerializeField] private int _damageFlatBonus;

    public override void ModifyDamage(ref DamageInformation damageInformation)
    {
        damageInformation.FlatBonusDamage += _damageFlatBonus;
    }
}
