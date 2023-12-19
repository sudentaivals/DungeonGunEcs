using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/Towards mouse")]
public class RotateTowardsMouse :ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {       
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var hostTransform = ref transformPool.Get(sender);

        var mousePos = Input.mousePosition;
        var mouseToWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        mouseToWorldPos.z = 0;
        var playerMouseVector = mouseToWorldPos - hostTransform.Transform.position;
        var angle = Mathf.Atan2(playerMouseVector.y, playerMouseVector.x);
        var angleDeg = angle * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angleDeg);
    }
}
