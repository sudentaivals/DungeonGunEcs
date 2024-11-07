using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Add or remove skill")]
public class AddOrRemoveSkill : GameAction
{
    [SerializeField] int _skillId;
    [SerializeField] bool _removeSkill;
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        if(_removeSkill)
        {
            var arg = EventArgsObjectPool.GetArgs<RemoveSkillEventArgs>();
            arg.SkillId = _skillId;
            EcsEventBus.Publish(GameplayEventType.ObjectRemoveSkill, senderEntity, arg);
        }
        else
        {
            var arg = EventArgsObjectPool.GetArgs<LearnSkillEventArgs>();
            arg.SkillId = _skillId;
            EcsEventBus.Publish(GameplayEventType.ObjectLearnSkill, senderEntity, arg);
        } 
    }
}
