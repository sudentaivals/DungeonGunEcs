using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HealthComponent 
{
    public bool IsAlive => CurrentHealth > 0;
    public int MaxHealth => BaseHealth + MaxHealthBonus;
    public int CurrentHealth;
    public int MaxHealthBonus;
    public int BaseHealth;

    //evasion
    public float GetClampedEvasion => Mathf.Clamp(EvasionChance, 0f, 0.95f);
    public float EvasionChance;

    //damage reduction
    public float DamageReductionPercent;
    public int DamageReductionFlat;

    //events
    public List<GameAction> OnDeath;
    public List<GameAction> OnEvasion;
    public List<GameAction> OnDamageTake;

}
