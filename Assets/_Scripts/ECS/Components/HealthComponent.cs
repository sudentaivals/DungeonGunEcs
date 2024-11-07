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
    public float GetClampedEvasion => Mathf.Clamp(EvasionChance, 0f, Settings.MaxEvasionChance);
    public float EvasionChance;

    //events
    public List<GameAction> OnDeath;
    public List<GameAction> OnEvasion;
    public List<GameAction> OnDamageTake;
    public List<DamageModificatorContainer> IncomingDamageModificators;
    public List<GameAction> ActionsOnDamageTake;

}
