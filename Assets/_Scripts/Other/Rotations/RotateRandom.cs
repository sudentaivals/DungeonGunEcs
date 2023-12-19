using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/Random")]
public class RotateRandom : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        return Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(-180, 180)));
    }
}
