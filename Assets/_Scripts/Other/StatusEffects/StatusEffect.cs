using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    private float _duration;
    private float _updateTimer;
    private float _baseUpdateTimer = 0f;
    private int _numberOfUpdates;
    //settings
    private int _id;
    private string _name;
    private string _description;
    private bool _isDebuff;
    //entities
    private int _hostEntity;
    private int _senderEntity;
    //stats
    private readonly GlobalStatsComponent _senderStats;
    //actions
    private List<GameAction> _actionsOnApply;
    private List<GameAction> _actionsOnRemove;
    private List<GameAction> _actionsOnUpdate;
    private List<GameAction> _onStacksIncreased;
    //stacks
    private bool _isStackable;
    private bool _isStacksRefreshDuration;
    private int _maxStacks;

    public int Id => _id;
    public int SenderEntity => _senderEntity;

    public bool IsOver => _duration <= 0;

    public StatusEffect(StatusEffectShell shell, int takerEntity, int senderEntity, bool overrideDuration = false, float newDuration = 0f)
    {
        _hostEntity = takerEntity;
        _senderEntity = senderEntity;
        _isDebuff = shell.IsDebuff;
        //stacks
        _isStackable = shell.IsStackable;
        _maxStacks = shell.MaxNumberOfStacks;
        _isStacksRefreshDuration = shell.IsStacksRefreshEffect;
        //snapshot sender stats
        _senderStats = EcsStart.World.GetPool<GlobalStatsComponent>().Get(senderEntity);
        _id = shell.Id;
        //name and description
        _name = shell.Name;
        _description = shell.Description;
        //update timers
        _numberOfUpdates = shell.NumberOfUpdates;
        _duration = overrideDuration ? newDuration : shell.Duration;
        _actionsOnApply = shell.ActionsOnApply;
        _actionsOnRemove = shell.ActionsOnRemove;
        _actionsOnUpdate = shell.ActionsOnUpdate;
        if(shell.NumberOfUpdates > 0)
        {
            if(shell.NumberOfUpdates == 1)
            {
                _baseUpdateTimer = _duration / 2f;
                _updateTimer = _baseUpdateTimer;
            }
            else
            {
                _baseUpdateTimer = _duration / (float)shell.NumberOfUpdates;
                _updateTimer = _baseUpdateTimer;
            }
        }
        OnApply();
    }

    public void UpdateStatusEffect(float delta)
    {
        if(_baseUpdateTimer > float.Epsilon)
        {
            _updateTimer -= delta;
            if (_updateTimer <= 0)
            {
                OnUpdate();
                _numberOfUpdates--;
                _updateTimer = _baseUpdateTimer;
            }
        }
        _duration -= delta;
        if (IsOver)
        {
            if (_numberOfUpdates != 0) OnUpdate();
            OnRemove();
        };
    }

    public void OnApply()
    {
        foreach (var action in _actionsOnApply)
        {
            action?.Action(_senderEntity, _hostEntity);
        }
    }

    public void OnRemove()
    {
        foreach (var action in _actionsOnRemove)
        {
            action?.Action(_senderEntity, _hostEntity);
        }
    }

    public void OnUpdate()
    {
        foreach (var action in _actionsOnUpdate)
        {
            action?.Action(_senderEntity, _hostEntity);
        }
    }

    public void OnStacksIncreased()
    {
        foreach (var action in _onStacksIncreased)
        {
            action.Action(_senderEntity, _hostEntity);
        }
    }
}
