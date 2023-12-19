using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SkillsComponent
{
    public List<(int, float)> Cooldowns;
    public List<int> LearnedSkills;
}