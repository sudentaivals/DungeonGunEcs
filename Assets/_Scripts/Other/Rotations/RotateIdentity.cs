using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/Identity")]
public class RotateIdentity : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        return Quaternion.identity;
    }
}
