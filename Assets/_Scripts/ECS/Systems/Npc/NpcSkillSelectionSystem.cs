using System;
using System.Linq;
using Leopotam.EcsLite;

public class NpcSkillSelectionSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;

    private EcsPool<NpcSkillSelectionComponent> _npcSkillSelectionPool;

    private EcsPool<SkillsComponent> _skillsPool;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.NpcUseSkill, UseNpcSkill);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NpcSkillSelectionComponent>()
                       .Inc<SkillsComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _npcSkillSelectionPool = world.GetPool<NpcSkillSelectionComponent>();
        _skillsPool = world.GetPool<SkillsComponent>();

        EcsEventBus.Subscribe(GameplayEventType.NpcUseSkill, UseNpcSkill);
    }

    private void UseNpcSkill(int entity, EventArgs args)
    {
        ref var npcSkillSelectionComponent = ref _npcSkillSelectionPool.Get(entity);
        npcSkillSelectionComponent.IsSkillSelected = false;
        var arg = EventArgsObjectPool.GetArgs<UseSkillEventArgs>();
        arg.SkillId = npcSkillSelectionComponent.CurrentSelectedNpcSkillId;
        EcsEventBus.Publish(GameplayEventType.ObjectUseSkill, entity, arg);

    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            CreateReadyToUseSkillsList(entity);
            CheckSelectedSkill(entity);
            SelectSkill(entity);
        }
    }

    private void CheckSelectedSkill(int entity)
    {
        ref var npcSkillSelectionComponent = ref _npcSkillSelectionPool.Get(entity);
        if(!npcSkillSelectionComponent.IsSkillSelected) return;
        if(npcSkillSelectionComponent.ReadyToUseSkills.Contains(npcSkillSelectionComponent.CurrentSelectedNpcSkillId)) return;
        npcSkillSelectionComponent.IsSkillSelected = false;
    }

    private void CreateReadyToUseSkillsList(int entity)
    {
        ref var npcSkillSelectionComponent = ref _npcSkillSelectionPool.Get(entity);
        ref var skillsComponent = ref _skillsPool.Get(entity);
        //filter cooldowns then filter with conditions
        var readyToUseSKills = skillsComponent.LearnedSkills.Except(skillsComponent.Cooldowns.Select(c => c.Item1))
                                                            .Where(skillId => SkillSystem.SkillIdToSkillShells[skillId].ConditionsAreValid(entity))
                                                            .OrderByDescending(skillId => SkillSystem.SkillIdToSkillShells[skillId].Priority)
                                                            .ToList();
        npcSkillSelectionComponent.ReadyToUseSkills = readyToUseSKills;
    }

    private void SelectSkill(int entity)
    {
        ref var npcSkillSelectionComponent = ref _npcSkillSelectionPool.Get(entity);
        if(npcSkillSelectionComponent.IsSkillSelected) return;
        if(npcSkillSelectionComponent.ReadyToUseSkills.Count == 0) return;
        npcSkillSelectionComponent.CurrentSelectedNpcSkillId = npcSkillSelectionComponent.ReadyToUseSkills.First();
        npcSkillSelectionComponent.IsSkillSelected = true;

    }
}
