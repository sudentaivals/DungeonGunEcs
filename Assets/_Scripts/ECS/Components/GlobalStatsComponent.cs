using System;
using UnityEngine;

[Serializable]
public struct GlobalStatsComponent
{
    //global stats
    public bool IsImmune;
    public Faction Faction;
    public UnitType UnitType;

    //health component
    /*public bool IsAlive => CurrentHealth > 0;
    public int MaxHealth => BaseHealth + MaxHealthBonus;
    public int CurrentHealth;
    public int MaxHealthBonus;
    public int BaseHealth;
    */
    //damage reduction MOVE TO HEALTH COMPONENT
    //public float GetClampedEvasion => Mathf.Clamp(EvasionChance, 0f, 0.95f);
    //public float EvasionChance;
    //public float DamageReductionPercent;
    //public int DamageReductionFlat;


    //damage bonus
    public float DamageBonusPercent;
    public int DamageBonusFlat;
    //movement
    //public Vector2 Direction;
    //public Vector2 Movement;
    //public Vector2 Push;
    //public float PushResistance;
    //public float Friction;
    //public float MovementSpeed;
    //public float Acceleration;
    //public float Decceleration;
    //public bool IsMovementAvailable;

    //skill stats
    public float CooldownReduction;
}
