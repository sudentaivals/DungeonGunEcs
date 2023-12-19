using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/PositioType/Mouse position")]
public class MousePosition : ScriptableObject, IPositionType
{
    public Vector2 GetPosition(int sender, int? taker)
    {
        var mousePos = Input.mousePosition;
        var mouseToWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseToWorldPos.z = 0;
        var position = mouseToWorldPos;
        return position;
    }
}
