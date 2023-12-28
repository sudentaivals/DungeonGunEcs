using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Raycast Type/Linecast")]
public class RaycastLine : ScriptableObject, IRaycastType
{
    private RaycastHit2D[] _results = new RaycastHit2D[1];
    public int GetRaycastResults(LayerMask mask, Vector2 startPoint, Vector2 direction, float distance, float raycastShapeData1 = 0, float raycastShapeData2 = 0)
    {
       return Physics2D.LinecastNonAlloc(startPoint, startPoint + direction * distance, _results, mask);
    }
}
