using System;
using System.Collections.Generic;

public class DamageInformation : IDeepClonable<DamageInformation>
{
    public int FinalDamage;
    public int InitialDamage;
    public int FlatBonusDamage;
    public float DamageMultiplier;
    public Guid DamageId;
    public float CriticalHitMultiplierBonus;
    public float CriticalHitBonus;
    public DamageType DamageType;
    public List<float> DamageMultiplicativeBonus = new();
    public bool IsCriticalHit;

    public DamageInformation DeepClone()
    {
        DamageInformation clone = new()
        {
            InitialDamage = this.InitialDamage,
            FlatBonusDamage = this.FlatBonusDamage,
            DamageMultiplier = this.DamageMultiplier,
            DamageId = this.DamageId,
            CriticalHitMultiplierBonus = this.CriticalHitMultiplierBonus,
            CriticalHitBonus = this.CriticalHitBonus,
            DamageType = this.DamageType,
            IsCriticalHit = this.IsCriticalHit,
            FinalDamage = this.FinalDamage
        };
        return clone;
    }
    public void UpdateVariables(int initialDamage, Guid damageId, DamageType damageType)
    {
        InitialDamage = initialDamage;
        DamageId = damageId;
        DamageType = damageType;
        CriticalHitMultiplierBonus = 0f;
        CriticalHitBonus = 0f;
        FlatBonusDamage = 0;
        DamageMultiplier = 0f;
        IsCriticalHit = false;
        FinalDamage = 0;
        DamageMultiplicativeBonus.Clear();
    }

    override public string ToString()
    {
        return $"InitialDamage: {InitialDamage},\nDamageId: {DamageId},\nDamageType: {DamageType},\nIsCriticalHit: {IsCriticalHit},\nCriticalHitMultiplierBonus: {CriticalHitMultiplierBonus},\nCriticalHitBonus: {CriticalHitBonus},\nFlatBonusDamage: {FlatBonusDamage},\nDamageMultiplier: {DamageMultiplier}";
    }
}
