using UnityEngine;

public interface IRaycastType
{
    public int GetRaycastResults(LayerMask mask, Vector2 startPoint, Vector2 direction, float distance, float raycastShapeData1 = 0f, float raycastShapeData2 = 0f);
}
