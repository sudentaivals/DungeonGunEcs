using System;
using System.Collections;
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
}
