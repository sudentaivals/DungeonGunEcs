using Leopotam.EcsLite;
using System;

public class PlayerSkillControllerSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<SkillsComponent> _skillsPool;
    private EcsPool<PlayerSkillComponent> _playerSkillsPool;
    private EcsPool<PlayerInputComponent> _playerInputPool;

    private readonly int _emptySkillId = 0;

    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _skillsPool = null;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<SkillsComponent>().Inc<PlayerInputComponent>().End();

        _skillsPool = world.GetPool<SkillsComponent>();
        _playerInputPool = world.GetPool<PlayerInputComponent>();
        _playerSkillsPool = world.GetPool<PlayerSkillComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            CheckForNullActiveSkill(entity);
            PlayerUseMainSkill(entity);
            PlayerUseAlternateSkill(entity);
        }
    }

    private void PlayerUseAlternateSkill(int entity)
    {
        ref var playerInput = ref _playerInputPool.Get(entity);
        if(!playerInput.IsAlternateShooting) return;

        ref var playerSkills = ref _playerSkillsPool.Get(entity);
        if (playerSkills.AlternateSkillId == _emptySkillId) return;

        var arg = EventArgsObjectPool.GetArgs<UseSkillEventArgs>();
        arg.SkillId = playerSkills.AlternateSkillId;
        EcsEventBus.Publish(GameplayEventType.ObjectUseSkill, entity, arg);


    }

    private void CheckForNullActiveSkill(int entity)
    {
        ref var skill = ref _skillsPool.Get(entity);
        ref var playerSkills = ref _playerSkillsPool.Get(entity);
        if (playerSkills.CurrentSelectedSkillId == _emptySkillId)
        {
            playerSkills.CurrentSelectedSkillId = skill.LearnedSkills[0];
            var arg = EventArgsObjectPool.GetArgs<SelectSkillEventArgs>();
            arg.SkillId = playerSkills.CurrentSelectedSkillId;
            EcsEventBus.Publish(GameplayEventType.ObjectSelectSKill, entity, arg);
        }
    }

    private void SelectNextSkill(int entity, EventArgs args)
    {
        ref var skill = ref _skillsPool.Get(entity);
        if(skill.LearnedSkills.Count == 0 || skill.LearnedSkills.Count == 1) return;
        ref var playerSkills = ref _playerSkillsPool.Get(entity);
        int currentIndex = skill.LearnedSkills.IndexOf(playerSkills.CurrentSelectedSkillId);
        if(currentIndex == skill.LearnedSkills.Count - 1)
        {
            playerSkills.CurrentSelectedSkillId = skill.LearnedSkills[0];
        }
        else
        {
            playerSkills.CurrentSelectedSkillId = skill.LearnedSkills[currentIndex + 1];
        }
        var arg = EventArgsObjectPool.GetArgs<SelectSkillEventArgs>();
        arg.SkillId = playerSkills.CurrentSelectedSkillId;
        EcsEventBus.Publish(GameplayEventType.ObjectSelectSKill, entity, arg);
    }

    private void SelectPreviousSkill(int entity, EventArgs args)
    {
        ref var skill = ref _skillsPool.Get(entity);
        if (skill.LearnedSkills.Count == 0 || skill.LearnedSkills.Count == 1) return;
        ref var playerSkills = ref _playerSkillsPool.Get(entity);
        int currentIndex = skill.LearnedSkills.IndexOf(playerSkills.CurrentSelectedSkillId);
        if (currentIndex == 0)
        {
            playerSkills.CurrentSelectedSkillId = skill.LearnedSkills[skill.LearnedSkills.Count - 1];
        }
        else
        {
            playerSkills.CurrentSelectedSkillId = skill.LearnedSkills[currentIndex - 1];
        }
        var arg = EventArgsObjectPool.GetArgs<SelectSkillEventArgs>();
        arg.SkillId = playerSkills.CurrentSelectedSkillId;
        EcsEventBus.Publish(GameplayEventType.ObjectSelectSKill, entity, arg);
    }

    private void PlayerUseMainSkill(int entity)
    {
        ref var playerInput = ref _playerInputPool.Get(entity);
        if(!playerInput.IsShooting && !playerInput.IsAutomaticShooting) return;
        //ref var skills = ref _skillsPool.Get(entity);
        ref var playerSkills = ref _playerSkillsPool.Get(entity);
        if (playerSkills.CurrentSelectedSkillId == _emptySkillId) return;

        var arg = EventArgsObjectPool.GetArgs<PlayerUseSkillEventArgs>();
        arg.SkillId = playerSkills.CurrentSelectedSkillId;
        arg.IsAutomatic = playerInput.IsAutomaticShooting;
        arg.IsPressed = playerInput.IsShooting;
        EcsEventBus.Publish(GameplayEventType.PlayerUseSkill, entity, arg);
    }
}
