using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/RotationType/As caster")]
public class RotateAsCaster : ScriptableObject, IRotationType
{
    public Quaternion GetRotation(int sender, int? taker)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var hostTransform = ref transformPool.Get(sender);
        var rotation = hostTransform.Transform.rotation;
        if(hostTransform.Transform.localScale.x < 0) rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        return rotation;
    }
}
