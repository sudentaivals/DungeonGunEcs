using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Npc Skill Is Selected")]
public class NpcSkillIsSelected : BaseGameCondition
{
    [SerializeField] bool _isSkillSelected;

    public override bool CheckCondition(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionArgs = null)
    {
        var npcSkillSelectionComponent = EcsStart.World.GetPool<NpcSkillSelectionComponent>();
        if(!npcSkillSelectionComponent.Has(senderEntity)) return false;

        ref var npcSkill = ref npcSkillSelectionComponent.Get(senderEntity);
        if(_isSkillSelected) return npcSkill.IsSkillSelected == true;
        else return npcSkill.IsSkillSelected == false;
    }
}
