using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Corpse copy owner action")]
public class CorpseCopyTransformAndSpriteAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity)
    {
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        var materialpool = EcsStart.World.GetPool<MaterialComponent>();
         
    }
}
