using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : IDeepClonable<Room>
{
    public string NodeId;
    public string TemplateId;
    public GameObject Prefab;
    public RoomNodeTypeSO RoomNodeType;
    public Vector2Int LowerBoundsWorld;
    public Vector2Int UpperBoundsWorld;
    public Vector2Int TemplateLowerBounds;
    public Vector2Int TemplateUpperBounds;
    public List<Vector2Int> SpawnPositions;
    public List<string> ChildRoomIdList;
    public string ParentRoomId;
    public List<Doorway> Doorways;
    public bool IsPositioned;
    public InstantiatedRoom InstantiatedRoom;
    public bool IsLit;
    public bool IsPreviouslyVisited;
    public bool IsClearedOfEnemies;

    public Room()
    {
        ChildRoomIdList = new();
        Doorways = new();
        SpawnPositions = new();
    }

    public bool IsOverlapping(Room otherRoom)
    {
        bool overlapX = Mathf.Max(LowerBoundsWorld.x, otherRoom.LowerBoundsWorld.x) <= Mathf.Min(UpperBoundsWorld.x, otherRoom.UpperBoundsWorld.x);
        if(overlapX == false) return false;
        bool overlapY = Mathf.Max(LowerBoundsWorld.y, otherRoom.LowerBoundsWorld.y) <= Mathf.Min(UpperBoundsWorld.y, otherRoom.UpperBoundsWorld.y);
        if(overlapY == false) return false;
        return true;
    }

    public Room DeepClone()
    {
        Room newRoom = new();

        newRoom.NodeId = NodeId;
        newRoom.TemplateId = TemplateId;
        newRoom.Prefab = Prefab;
        newRoom.RoomNodeType = RoomNodeType;
        newRoom.LowerBoundsWorld = LowerBoundsWorld;
        newRoom.UpperBoundsWorld = UpperBoundsWorld;
        newRoom.TemplateLowerBounds = TemplateLowerBounds;
        newRoom.TemplateUpperBounds = TemplateUpperBounds;
        newRoom.ChildRoomIdList = new List<string>(ChildRoomIdList);
        newRoom.ParentRoomId = ParentRoomId;
        newRoom.Doorways = new List<Doorway>(Doorways);
        newRoom.IsPositioned = IsPositioned;
        newRoom.InstantiatedRoom = InstantiatedRoom;
        newRoom.IsLit = IsLit;
        newRoom.IsPreviouslyVisited = IsPreviouslyVisited;
        newRoom.IsClearedOfEnemies = IsClearedOfEnemies;
        return newRoom;
    }
}
