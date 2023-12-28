using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/No direction")]
public class NoDirection : ScriptableObject, IMovementDirection
{
    public Vector2 GetDirection(int sender)
    {
        return Vector2.zero;
    }
}
