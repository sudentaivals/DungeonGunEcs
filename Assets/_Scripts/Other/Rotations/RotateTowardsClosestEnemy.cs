using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/Towards closest enemy")]
public class RotateTowardsClosestEnemy :ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        return Quaternion.identity;
    }
}
