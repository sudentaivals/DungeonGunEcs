using Leopotam.EcsLite;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : IEcsRunSystem, IEcsPreInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<SkillsComponent> _skillsPool;

    private static Dictionary<int, SkillShell> _skillIdToSkillShells;

    public static Dictionary<int, SkillShell> SkillIdToSkillShells => _skillIdToSkillShells;
    readonly string RESOURCES_SKILLS_PATH = "Skills";

    private void LearnNewSkill(int entity, EventArgs args)
    {
        var skillArgs = args as LearnSkillEventArgs;
        ref var objectSkills = ref _skillsPool.Get(entity);
        //check for collection null
        if(objectSkills.LearnedSkills == null) objectSkills.LearnedSkills = new List<int>();
        //check for skill existance
        if(objectSkills.LearnedSkills.Contains(skillArgs.SkillId)) return;

        objectSkills.LearnedSkills.Add(skillArgs.SkillId);

        var skillShell = _skillIdToSkillShells[skillArgs.SkillId];
        skillShell.OnSkillLearn(entity);
        

        //objectSkills.SkillsShell.Add(skillArgs.SkillShell);
    }

    private void RemoveSkill(int entity, EventArgs args)
    {
        var removeSkillArgs = args as RemoveSkillEventArgs;
        ref var objectSkills = ref _skillsPool.Get(entity);

        if(objectSkills.LearnedSkills == null) objectSkills.LearnedSkills = new List<int>();
        if (!objectSkills.LearnedSkills.Contains(removeSkillArgs.SkillId)) return;
        objectSkills.LearnedSkills.Remove(removeSkillArgs.SkillId);

        var skillShell = _skillIdToSkillShells[removeSkillArgs.SkillId];
        skillShell.OnSkillLost(entity);
    }

    private void SelectSkill(int entity, EventArgs args)
    {
        var selectSkillArgs = args as SelectSkillEventArgs;
        ref var objectSkills = ref _skillsPool.Get(entity);
        var skillShell = _skillIdToSkillShells[selectSkillArgs.SkillId];
        skillShell.OnSkillSelect(entity);
    }

    private void UseSkill(int entity, EventArgs args)
    {
        var useSkillArgs = args as UseSkillEventArgs;
        ref var objectSkills = ref _skillsPool.Get(entity);

        //check cooldown
        if(objectSkills.Cooldowns.Select(a => a.Item1).Contains(useSkillArgs.SkillId)) return;
        //check conditions
        var skill = _skillIdToSkillShells[useSkillArgs.SkillId];
        if(skill.Conditions.Any(a => a.CheckCondition(entity, null) == false)) return;
        //use skill
        skill.OnSkilllUse(entity);
        //add cooldown
        objectSkills.Cooldowns.Add(new (useSkillArgs.SkillId, skill.BaseCooldown));
    }

    private void PlayerUseSkill(int entity, EventArgs args)
    {
        var useSkillArgs = args as PlayerUseSkillEventArgs;
        ref var objectSkills = ref _skillsPool.Get(entity);

        //check cooldown
        if(objectSkills.Cooldowns.Select(a => a.Item1).Contains(useSkillArgs.SkillId)) return;
        //check conditions
        var skill = _skillIdToSkillShells[useSkillArgs.SkillId];
        if(skill.Conditions.Any(a => a.CheckCondition(entity, null) == false)) return;
        //use skill
        var skillShell = _skillIdToSkillShells[useSkillArgs.SkillId];

        if (skillShell.IsAutomatic && useSkillArgs.IsAutomatic)
        {
            skillShell.OnSkilllUse(entity);
        }
        else if (skillShell.IsAutomatic && useSkillArgs.IsPressed)
        {
            skillShell.OnSkilllUse(entity);
        }
        //add cooldown
        objectSkills.Cooldowns.Add(new (useSkillArgs.SkillId, skill.BaseCooldown));
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var skillComp = ref _skillsPool.Get(entity);
            if(skillComp.Cooldowns == null) skillComp.Cooldowns = new();
            //change cooldown
            ChangeSkillCooldown(entity);
        }
    }
    private void ChangeSkillCooldown(int entity)
    {
        ref var objectSkills = ref _skillsPool.Get(entity);

        for (int i = 0; i < objectSkills.Cooldowns.Count; i++)
        {
            if(objectSkills.Cooldowns[i].Item2 > 0) 
            objectSkills.Cooldowns[i] = new (objectSkills.Cooldowns[i].Item1, objectSkills.Cooldowns[i].Item2 - Time.deltaTime);
        }
        objectSkills.Cooldowns.RemoveAll(cooldown => cooldown.Item2 <= 0f);
    }

    private void GetSkillShells()
    {
        _skillIdToSkillShells = new();
        _skillIdToSkillShells = Resources.LoadAll<SkillShell>(RESOURCES_SKILLS_PATH).ToDictionary(a => a.Id);
        foreach (var item in _skillIdToSkillShells);
    }

    public void PreInit(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<SkillsComponent>().Exc<PooledObjectTag>().End();
        _skillsPool = world.GetPool<SkillsComponent>();

        GetSkillShells();
        EcsEventBus.Subscribe(GameplayEventType.ObjectLearnSkill, LearnNewSkill);
        EcsEventBus.Subscribe(GameplayEventType.ObjectRemoveSkill, RemoveSkill);
        EcsEventBus.Subscribe(GameplayEventType.ObjectSelectSKill, SelectSkill);
        EcsEventBus.Subscribe(GameplayEventType.ObjectUseSkill, UseSkill);
        EcsEventBus.Subscribe(GameplayEventType.PlayerUseSkill, PlayerUseSkill);
        
    }

    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _skillsPool = null;
        _skillIdToSkillShells = null;
        EcsEventBus.Unsubscribe(GameplayEventType.ObjectLearnSkill, LearnNewSkill);
        EcsEventBus.Unsubscribe(GameplayEventType.ObjectRemoveSkill, RemoveSkill);
        EcsEventBus.Unsubscribe(GameplayEventType.ObjectSelectSKill, SelectSkill);
        EcsEventBus.Unsubscribe(GameplayEventType.ObjectUseSkill, UseSkill);
        EcsEventBus.Unsubscribe(GameplayEventType.PlayerUseSkill, PlayerUseSkill);
    }
}

