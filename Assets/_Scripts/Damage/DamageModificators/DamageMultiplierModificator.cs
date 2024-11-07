using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/DamageMod/Modificators/New damage multiplier")]
public class DamageMultiplierModificator : DamageModificator
{
    [SerializeField] private float _damageMultiplier;
    public override void ModifyDamage(ref DamageInformation damageInformation)
    {
        damageInformation.DamageMultiplier += _damageMultiplier;
    }
}
