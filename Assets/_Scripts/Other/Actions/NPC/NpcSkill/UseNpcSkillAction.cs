using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/NPC/Use npc skill")]
public class UseNpcSkillAction : GameAction
{
    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        EcsEventBus.Publish(GameplayEventType.NpcUseSkill, senderEntity, null);
    }
}
