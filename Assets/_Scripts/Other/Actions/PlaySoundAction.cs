using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Play sound")]
public class PlaySoundAction : GameAction
{
    [SerializeField] AudioClip _sound;
    [Range(0f, 1f)]
    [SerializeField] float _soundVolume;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var args = EventArgsObjectPool.GetArgs<PlaySoundEventArgs>();
        args.Sfx = _sound;
        args.Volume = _soundVolume;
        EcsEventBus.Publish(GameplayEventType.PlaySound, senderEntity, args);
    }
}
