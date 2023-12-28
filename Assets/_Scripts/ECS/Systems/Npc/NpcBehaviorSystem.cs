using Leopotam.EcsLite;
using System;
using System.Linq;
using UnityEngine;

public class NpcBehaviorSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<NpcStateComponent> _npsStatePool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<NpcStateComponent>()
                       .Exc<PooledObjectTag>()
                       .End();
        _npsStatePool = world.GetPool<NpcStateComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            //ref var npcState = ref _npsStatePool.Get(entity);
            if(CheckUpdateTime(entity))
            {
                ChooseState(entity);
            }
            UpdateState(entity);
        }
    }

    private bool CheckUpdateTime(int entity)
    {
        ref var npcState = ref _npsStatePool.Get(entity);
        npcState.CurrentUpdateDelay -= Time.deltaTime;
        return npcState.CurrentUpdateDelay <= 0;
    }

    private void ChooseState(int entity)
    {
        ref var npcState = ref _npsStatePool.Get(entity);
        int newStateId = 0;
        //var newState = npcState.States.FirstOrDefault(a => a.AllStartConditionsValid(entity));
        //if stateId <= 0 then currentState = baseState, else - select new state from transitions
        if (npcState.RunningStateId <= 0)
        {
            newStateId = npcState.FSM.BaseState.Id;
        }
        else
        {
            //select transition
            int transitionId = npcState.RunningStateId;
            var branch = npcState.FSM.Branches.First(a => a.SelectedState.Id == transitionId);
            //check conditions in transition list
            var sequence = branch.Transitions.FirstOrDefault(a => a.AllStateConditionsValid(entity));
            if (sequence == null) newStateId = npcState.FSM.BaseState.Id;
            else
            {
                newStateId = sequence.State.Id;
            }
        }
        //same state? check exit conditions; if not => return;
        if (npcState.RunningStateId == newStateId)
        {
            var runningsState = npcState.FSM.Branches.FirstOrDefault(a => a.SelectedState.Id == newStateId);
            var exitConditionsAreMet = runningsState.SelectedState.AllEndConditionsValid(entity);
            if(exitConditionsAreMet) newStateId = npcState.FSM.BaseState.Id;
            else return;
        }
        //old state exit actions; no meeting criteria for exit state => change it
        int currentStateId = npcState.RunningStateId;
        var currentState = npcState.FSM.Branches.FirstOrDefault(a => a.SelectedState.Id == currentStateId);
        if (currentState != null) currentState.SelectedState.PerformActionsOnEnd(entity);
        //new state enter actions
        npcState.RunningStateId = newStateId;
        npcState.FSM.Branches.First(a => a.SelectedState.Id == newStateId).SelectedState.PerformActionsOnStart(entity);
        npcState.CurrentUpdateDelay = npcState.UpateDelay;
        //Debug.Log(npcState.RunningStateId);
        //newState.PerformActionsOnStart(entity);
    }

    private void UpdateState(int entity)
    {
        ref var npcState = ref _npsStatePool.Get(entity);
        int currentStateId = npcState.RunningStateId;
        var currentState = npcState.FSM.Branches.FirstOrDefault(a => a.SelectedState.Id == currentStateId);
        if (currentState != null) currentState.SelectedState.PerformActionsOnUpdate(entity);
    }
}
