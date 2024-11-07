using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    [Tooltip("Populate with dungeon level script objects")]
    [SerializeField] private List<DungeonLevelSO> _dungeonLevels;
    [Tooltip("Starting level for testing, default = 0")]
    [SerializeField] private int _currentDungeonLevelIndex = 0;
    [SerializeField] private GameStateSO _currentState;
    [SerializeField] private GameStateSO _defaultState;

    public GameStateSO CurrentState => _currentState;

    private void Start() 
    {
        ChangeGameState(_defaultState);
    }

    private void Update() 
    {
        HandleGameState();
    }

    private void OnEnable()
    {
        EcsEventBus.Subscribe(GameplayEventType.ChangeGameState, ChangeGameState);
        EcsEventBus.Subscribe(GameplayEventType.PlayDungeonLevel, PlayDungeonLevel);
    }

    private void OnDisable()
    {
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeGameState, ChangeGameState);
        EcsEventBus.Unsubscribe(GameplayEventType.PlayDungeonLevel, PlayDungeonLevel);
    }

    private void ChangeGameState(int entity, EventArgs args)
    {
        var changeGameStateArgs = args as ChangeGameStateEventArgs;
        if(changeGameStateArgs == null) return;

        ChangeGameState(changeGameStateArgs.NewGameState);
    }

    private void ChangeGameState(GameStateSO newGameState)
    {
        if(_currentState == newGameState) return;
        if(_currentState != null) _currentState.EndActions();
        _currentState = newGameState;
        _currentState.StartActions();
    }

    private void PlayDungeonLevel(int entity, EventArgs args)
    {
        PlayDungeonLevel(_currentDungeonLevelIndex);
    }

    private void PlayDungeonLevel(int levelIndex)
    {
        bool dungeonBuildSuccesful = DungeonBuilder.Instance.GenerateDungeon(_dungeonLevels[levelIndex]);
        if(!dungeonBuildSuccesful)
        {
            Debug.LogError("Failed to build dungeon");
        }
    }

    private void HandleGameState()
    {
        if(_currentState == null) return;
        _currentState.Handle();
    }

    #region  VALIDATION
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(_dungeonLevels), _dungeonLevels);
    }
#endif
    #endregion
}
