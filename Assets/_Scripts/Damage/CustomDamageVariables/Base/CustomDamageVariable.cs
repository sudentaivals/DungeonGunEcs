using UnityEngine;

public class CustomDamageVariable : ScriptableObject
{
    public virtual int GetDamageValue(int damageDealerEntity, int? damageTakerEntity)
    {
        return 0;
    }
}
