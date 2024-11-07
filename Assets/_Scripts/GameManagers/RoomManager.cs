using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    private GameObject _room = null;
    private List<Vector2> _spawnPoints = new();
    private Vector3 _roomPosition;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable() 
    {
        EcsEventBus.Subscribe(GameplayEventType.SetActiveRoom, SetActiveRoom);
    }
    
    private void OnDisable() 
    {
        EcsEventBus.Unsubscribe(GameplayEventType.SetActiveRoom, SetActiveRoom);
    }

    private void SetActiveRoom(int sender, EventArgs args)
    {
        var roomArgs = args as ChangeActiveRoomEventArgs;
        _room = roomArgs.NewRoom;
        _spawnPoints = roomArgs.SpawnPoints;
        _roomPosition = _room.transform.position;
    }
}
