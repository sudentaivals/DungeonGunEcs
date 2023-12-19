using System;
using System.Collections.Generic;

[Serializable]
public struct NpcSkillSelectionComponent
{
    public List<int> ReadyToUseSkills;
    public int CurrentSelectedNpcSkillId;
    public bool IsSkillSelected;
}
