using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/FSM/FSM")]
public class FSM : ScriptableObject
{
    [SerializeField] NpcState _baseState;
    [SerializeField] List<FSMTransitions> _transitions;
    public NpcState BaseState => _baseState;
    public List<FSMTransitions> Branches => _transitions;
}

[Serializable]
public class FSMTransitions
{
    public NpcState SelectedState;
    public List<FSMTransition> Transitions;
}
