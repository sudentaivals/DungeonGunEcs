using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Status effect shell")]
public class StatusEffectShell : ScriptableObject
{
    [Header("Actions")]
    [SerializeField] List<GameAction> _actionsOnApply;
    [SerializeField] List<GameAction> _actionsOnRemove;
    [SerializeField] List<GameAction> _actionsOnUpdate;
    [SerializeField] List<GameAction> _onStacksIncreased;
    [Header("Settings")]
    [SerializeField] bool _isDebuff;
    [SerializeField] int _numberOfUpdates;
    [SerializeField] float _duration;
    [SerializeField] SkillApplyType _skillApplyType;
    [SerializeField] string _name;
    [SerializeField] int _id;
    [Header("Stacks")]
    [SerializeField] bool _isStackable;
    [SerializeField] bool _isStackRefreshEffect;
    [SerializeField] int _maxNumberOfStacks;
    [Header("UI")]
    [SerializeField] Sprite _icon;
    
    [TextArea(3, 10)]
    [SerializeField] string _description;



    //actions
    public List<GameAction> ActionsOnApply => _actionsOnApply;
    public List<GameAction> ActionsOnRemove => _actionsOnRemove;
    public List<GameAction> ActionsOnUpdate => _actionsOnUpdate;
    public List<GameAction> ActionsOnStacksIncreased => _onStacksIncreased;
    public SkillApplyType SkillApplyType => _skillApplyType;

    //settings
    public bool IsDebuff => _isDebuff;
    public float Duration => _duration;
    public int NumberOfUpdates => _numberOfUpdates;
    public string Name => _name;
    public int Id => _id;
    //stack
    public bool IsStackable => _isStackable;
    public bool IsStacksRefreshEffect => _isStackRefreshEffect;
    public int MaxNumberOfStacks => _maxNumberOfStacks;
    //UI
    public Sprite Icon => _icon;

    public string Description => _description;




}


public enum SkillApplyType
{
    UniqueForAll,
    UniquePerObject,
    Unlimited
}
