using UnityEngine;

public class DamageModificator : ScriptableObject
{
    public virtual void ModifyDamage(ref DamageInformation damageInformation)
    {
        return;
    }
}
