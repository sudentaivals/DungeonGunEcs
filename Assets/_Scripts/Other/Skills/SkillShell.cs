using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Skill shell")]
public class SkillShell : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] float _baseCooldown;
    [SerializeField] bool _affectedByCdReduction;
    [SerializeField] string _name;
    [SerializeField] int _id;
    
    [SerializeField] bool _isAutomactic = true;

    [SerializeField] bool _isUnique = false;
    [TextArea(3, 10)]
    [SerializeField] string _description;

    [SerializeField] List<BaseGameCondition> _conditions;

    [Header("AI")]
    [SerializeField] int _priority;
    //events
    [Header("Events")]
    [SerializeField] List<GameAction> _actionsOnSkillSelect;
    [SerializeField] List<GameAction> _actionsOnSkillLearn;
    [SerializeField] List<GameAction> _actionsOnSkillLost;
    [SerializeField] List<GameAction> _actionsOnUse;

    public float BaseCooldown => _baseCooldown;
    public bool AffectedByCdr => _affectedByCdReduction;
    public string Name => _name;
    public int Id => _id;
    public bool IsUnique => _isUnique;

    public bool IsAutomatic => _isAutomactic;


    public List<GameAction> ActionsOnUse => _actionsOnUse;
    public List<GameAction> ActionOnSelect => _actionsOnSkillSelect;
    public List<GameAction> ActionOnLearn => _actionsOnSkillLearn;
    public List<GameAction> ActionsOnLost => _actionsOnSkillLost;

    public List<BaseGameCondition> Conditions => _conditions;

    public int Priority => _priority;

    public bool ConditionsAreValid(int senderEntity)
    {
        return _conditions.All(condition => condition.CheckCondition(senderEntity, null));
    }

    public void OnSkillLearn(int senderEntity)
    {
        foreach (var action in _actionsOnSkillLearn)
        {
            action?.Action(senderEntity, null);
        }
    }

    public void OnSkillLost(int senderEntity)
    {
        foreach (var action in _actionsOnSkillLost)
        {
            action?.Action(senderEntity, null);
        }
    }

    public void OnSkilllUse(int senderEntity)
    {
        foreach (var action in _actionsOnUse)
        {
            action?.Action(senderEntity, null);
        }
    }

    public void OnSkillSelect(int senderEntity)
    {
        foreach (var action in _actionsOnSkillSelect)
        {
            action?.Action(senderEntity, null);
        }
    }
}

