using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    private int _hostEntity;
    //cooldowns
    private float _cooldown;
    private float _currentCooldown = 0;
    private bool _isAffectedByCdr;
    private bool _isAutomatic;
    public bool IsAutomatic => _isAutomatic;
    //targeting
    private float _castRange;
    //actions
    private List<GameAction> _actionsOnSkillUse;
    private List<GameAction> _actionsOnSkillSelect;
    private List<GameAction> _actionsOnSkillLearn;
    private List<GameAction> _actionsOnSkillLost;

    //id and name
    private string _name;
    private int _skillId;

    public int SkillId => _skillId;

    public string Name => _name;

    private bool IsSkillAvailable => _currentCooldown <= 0;
    public int HostEntity => _hostEntity;

    public Skill(SkillShell shell, int hostEntity)
    {
        //_targetType = shell.TargetType;

        _hostEntity = hostEntity;
        _cooldown = shell.BaseCooldown;
        _isAffectedByCdr = shell.AffectedByCdr;
        _actionsOnSkillUse = shell.ActionsOnUse;
        _actionsOnSkillLost = shell.ActionsOnLost;
        _actionsOnSkillSelect = shell.ActionOnSelect;
        _actionsOnSkillLearn = shell.ActionOnLearn;
        _skillId = shell.Id;
        _name = shell.Name;
        _isAutomatic = shell.IsAutomatic;
    }

    public bool UseSkill()
    {
        if (!IsSkillAvailable) return false;
        int? targetEntity = SelectTarget();
        //actions
        foreach (var skillAction in _actionsOnSkillUse)
        {
            skillAction?.Action(HostEntity, targetEntity);
        }
        //adding cooldown
        SetCooldown();
        return true;
    }
    private int? SelectTarget()
    {
        return null;
    }

    public void OnSkillLearn()
    {
        var target = SelectTarget();
        foreach (var action in _actionsOnSkillLearn)
        {
            action?.Action(HostEntity, target);
        }
    }

    public void OnSkillLost()
    {
        var target = SelectTarget();
        foreach (var action in _actionsOnSkillLost)
        {
            action?.Action(HostEntity, target);
        }
    }

    //player only
    public void OnSkillSelect()
    {
        var target = SelectTarget();
        foreach (var action in _actionsOnSkillSelect)
        {
            action?.Action(HostEntity, target);
        }
    }

    private void SetCooldown()
    {
        if (_isAffectedByCdr)
        {
            ref var ownerStats = ref EcsStart.World.GetPool<GlobalStatsComponent>().Get(_hostEntity);
            var cooldownAfterReduction = _cooldown - (_cooldown * ownerStats.CooldownReduction);
            _currentCooldown = cooldownAfterReduction;
        }
        else
        {
            _currentCooldown = _cooldown;
        }

    }

    public void ChangeCooldown(float delta)
    {
        if (!IsSkillAvailable)
        {
            _currentCooldown -= delta;
        }
    }
}
