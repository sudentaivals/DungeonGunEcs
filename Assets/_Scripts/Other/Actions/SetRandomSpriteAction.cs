using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Set random sprite from array")]
public class SetRandomSpriteAction : GameAction
{
    [SerializeField] List<Sprite> _sprites;
    public override void Action(int senderEntity, int? takerEntity)
    {
        int index = Random.Range(0, _sprites.Count);
        var args = EventArgsObjectPool.GetArgs<ChangeSpriteEventArgs>();
        args.Sprite = _sprites[index];
        EcsEventBus.Publish(GameplayEventType.ChangeSprite, senderEntity, args);

    }
}
