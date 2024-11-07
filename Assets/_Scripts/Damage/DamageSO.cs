using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageSO", menuName = "My Assets/DamageMod/DamageSO")]

public class DamageSO : ScriptableObject
{
    public Guid Guid => SerializableDamageGuid.Guid;
    [Header("Guid")]
    [SerializeField] private SerializableGuid SerializableDamageGuid;
    [Header("Damage variables")]
    [SerializeField] private int Damage;
    public DamageType DamageType;
    [SerializeField] private bool IsOverrideDamageValue = false;
    [SerializeField] private CustomDamageVariable CustomDamageVariable;
    public int GetDamageValue(int damageDealerEntity, int? damageTakerEntity)
    {
        if(!IsOverrideDamageValue) return Damage;
        if(CustomDamageVariable == null) return Damage;
        return CustomDamageVariable.GetDamageValue(damageDealerEntity, damageTakerEntity);
    }

}
