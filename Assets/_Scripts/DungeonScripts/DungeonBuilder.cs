using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DungeonBuilder : SingletonInstance<DungeonBuilder>
{
    public Dictionary<string, Room> DungeonBuilderRoomDictionary = new();
    private Dictionary<string, RoomTemplateSO> RoomTemplateDictionary = new();
    private List<RoomTemplateSO> _roomTemplateList = null;
    private RoomNodeTypeListSO _roomNodeTypeList = null;
    private bool _dungeonBuildSuccesful;

    public override void Awake()
    {
        base.Awake();
        LoadRoomNodeTypeList();
        GameResources.Instance.DimmedMaterial.SetFloat(ShaderParam.AlphaSlider, 1f);
    }

    private void LoadRoomNodeTypeList()
    {
        _roomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
    }

    public bool GenerateDungeon(DungeonLevelSO dungeonLevel)
    {
        _roomTemplateList = dungeonLevel.RoomTemplates;
        LoadRoomTemplatesIntoDictionary();

        _dungeonBuildSuccesful = false;
        int dungeonBuildAttempts = 0;
        while(true)
        {
            if(_dungeonBuildSuccesful) break;
            if(dungeonBuildAttempts >= Settings.MaxDungeonBuildAttempts) break;
            dungeonBuildAttempts++;
            RoomNodeGraphSO randomGraph = GetRandomRoomNodeGraph(dungeonLevel.RoomNodeGraphList);
            if(randomGraph == null) break;
            int currentGraphBuildAttempts = 0;
            while(true)
            {
                if(currentGraphBuildAttempts >= Settings.MaxDungeonBuildAttemptsForRoomGraph) break;
                ClearDungeon();
                currentGraphBuildAttempts++;
                _dungeonBuildSuccesful = AttemptToBuildDungeon(randomGraph);

                if(_dungeonBuildSuccesful)
                {
                    InstantiateRoomGameObjects();
                    break;
                }
            }
        }
        return _dungeonBuildSuccesful;
    }

    private void InstantiateRoomGameObjects()
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            if(room == null) continue;
            Vector3 roomPos = new Vector3(room.LowerBoundsWorld.x - room.TemplateLowerBounds.x, room.LowerBoundsWorld.y - room.TemplateLowerBounds.y, 0f);
            GameObject roomGameObject = Instantiate(room.Prefab, roomPos, Quaternion.identity, transform);
            InstantiatedRoom instantiatedRoom = roomGameObject.GetComponent<InstantiatedRoom>();
            instantiatedRoom.Room = room;
            //instantiatedRoom.Initialise(roomGameObject);
            room.InstantiatedRoom = instantiatedRoom;
            //instantiatedRoom.PathfindGrid.ActivateGrid();
        }
    }

    /// <summary>
    /// Attempt to build dungeon for specified room node graph.
    /// </summary>
    private bool AttemptToBuildDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        RoomNodeSO entranceRoom = roomNodeGraph.GetRoomNodeByType(_roomNodeTypeList.List.Find(roomNodeType => roomNodeType.IsEntrance));
        if(entranceRoom == null) return false;

        Queue<RoomNodeSO> roomNodeQueue = new();
        roomNodeQueue.Enqueue(entranceRoom);
        bool noRoomOverlaps = true;

        noRoomOverlaps = ProcessRoomsInQueue(roomNodeQueue, roomNodeGraph, noRoomOverlaps);

        if(noRoomOverlaps && roomNodeQueue.Count == 0) return true;
        return false;
    }

    private bool ProcessRoomsInQueue(Queue<RoomNodeSO> roomNodeQueue, RoomNodeGraphSO roomNodeGraph, bool noRoomOverlaps)
    {
        while(roomNodeQueue.Count > 0 && noRoomOverlaps == true)
        {
            RoomNodeSO roomNode = roomNodeQueue.Dequeue();
            foreach (RoomNodeSO childRoom in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                roomNodeQueue.Enqueue(childRoom);
            }
            //add entrance room to dictionary
            if(roomNode.RoomNodeType.IsEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.RoomNodeType);
                Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);
                room.IsPositioned = true;
                DungeonBuilderRoomDictionary.Add(room.NodeId, room);
            }
            else
            {
                Room parentRoom = DungeonBuilderRoomDictionary[roomNode.ParentRoomNodeIdList[0]];
                noRoomOverlaps = PlaceRoomIfValid(roomNode, parentRoom);
            }
        }
        return noRoomOverlaps;
    }

    /// <summary>
    /// Try to place room in dungeon with no overlaps. If overlap occur return false.
    /// </summary>
    private bool PlaceRoomIfValid(RoomNodeSO roomNode, Room parentRoom)
    {
        //it overlaps until proven otherwise
        bool roomOverlaps = true;

        while(roomOverlaps)
        {
            List<Doorway> unconnectedDoorways = parentRoom.Doorways.Where(doorway => doorway.IsUnavailable == false && doorway.IsConnected == false).ToList();

            if(unconnectedDoorways.Count == 0) return false;

            Doorway doorwayParent = unconnectedDoorways[UnityEngine.Random.Range(0, unconnectedDoorways.Count)];
            RoomTemplateSO roomTemplate = GetRandomTemplateConsistentWithParent(roomNode, doorwayParent);
            Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

            //place room - returns true if room not overlap
            if(TryPlaceRoom(parentRoom, doorwayParent, room))
            {
                roomOverlaps = false;

                room.IsPositioned = true;
                DungeonBuilderRoomDictionary.Add(room.NodeId, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }
        //no overlaps
        return true;
    }

    /// <summary>
    /// Return true if room doew not overlap
    /// </summary>
    private bool TryPlaceRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.Doorways);
        if (doorway == null)
        {
            doorwayParent.IsUnavailable = true;
            return false;
        }

        //world grid doorway position
        Vector2Int parentDoorwayPosition = parentRoom.LowerBoundsWorld + doorwayParent.Position - parentRoom.TemplateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;
        switch (doorway.Orientaion)
        {
            case Orientation.East:
                adjustment = new Vector2Int(-1, 0);
                break;
            case Orientation.West:
                adjustment = new Vector2Int(1, 0);
                break;
            case Orientation.North:
                adjustment = new Vector2Int(0, -1);
                break;
            case Orientation.South:
                adjustment = new Vector2Int(0, 1);
                break;
            case Orientation.None:
                break;
            default:
                break;
        }
        room.LowerBoundsWorld = parentDoorwayPosition + adjustment + room.TemplateLowerBounds - doorway.Position;
        room.UpperBoundsWorld = room.LowerBoundsWorld + room.TemplateUpperBounds - room.TemplateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);
        if(overlappingRoom != null)
        {
            doorwayParent.IsUnavailable = true;
            return false;
        }
        doorwayParent.IsConnected = true;
        doorwayParent.IsUnavailable = true;

        doorway.IsConnected = true;
        doorway.IsUnavailable = true;
        return true;
        
    }

    /// <summary>
    /// Check for room overlap, passing current room and looking for bounding box collisions
    /// </summary>
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilderRoomDictionary)
        {
            if(roomToTest.TemplateId == keyValuePair.Value.TemplateId) continue;
            if(roomToTest.IsOverlapping(keyValuePair.Value)) return keyValuePair.Value;
        }
        return null;
    }

    /// <summary>
    /// Get the doorway from doorway list that is opposite to doorwayParent
    /// </summary>
    private Doorway GetOppositeDoorway(Doorway doorwayParent, List<Doorway> doorways)
    {
        foreach (Doorway doorwayToCheck in doorways)
        {
            if(doorwayParent.Orientaion == Orientation.East && doorwayToCheck.Orientaion == Orientation.West)
            {
                return doorwayToCheck;
            }
            else if(doorwayParent.Orientaion == Orientation.West && doorwayToCheck.Orientaion == Orientation.East)
            {
                return doorwayToCheck;
            }
            else if(doorwayParent.Orientaion == Orientation.North && doorwayToCheck.Orientaion == Orientation.South)
            {
                return doorwayToCheck;
            }
            else if(doorwayParent.Orientaion == Orientation.South && doorwayToCheck.Orientaion == Orientation.North)
            {
                return doorwayToCheck;
            }
        }
        return null;
    }

    private RoomTemplateSO GetRandomTemplateConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;
        if(roomNode.RoomNodeType.IsCorridor)
        {
            switch(doorwayParent.Orientaion)
            {
                case Orientation.North:
                case Orientation.South:
                    roomTemplate = GetRandomRoomTemplate(_roomNodeTypeList.List.Find(roomNodeType => roomNodeType.IsCorridorNS));
                    break;

                case Orientation.West:
                case Orientation.East:
                    roomTemplate = GetRandomRoomTemplate(_roomNodeTypeList.List.Find(roomNodeType => roomNodeType.IsCorridorEW));
                    break;
                    
                case Orientation.None:
                    break;
                default:
                    break;
            }
        }
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.RoomNodeType);
        }
        return roomTemplate;
    }

    /// <summary>
    /// Iterate through all room templates and find one that matches the room node type.
    /// If no room template matches the room node type, return null.
    /// </summary>
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new();
        foreach(RoomTemplateSO roomTemplate in _roomTemplateList)
        {
            if(roomTemplate.RoomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }
        if(matchingRoomTemplateList.Count == 0) return null;
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];
    }

    /// <summary>
    /// Create room based on roomTemplate and layout node.
    /// </summary>
    private Room CreateRoomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        Room room = new();
        room.RoomNodeType = roomTemplate.RoomNodeType;
        room.TemplateId = roomTemplate.GuId;
        room.NodeId = roomNode.Id;
        room.Prefab = roomTemplate.RoomPrefab;
        room.UpperBoundsWorld = roomTemplate.UpperBounds;
        room.LowerBoundsWorld = roomTemplate.LowerBounds;
        room.SpawnPositions = roomTemplate.SpawnPositionsArray;
        room.TemplateLowerBounds = roomTemplate.LowerBounds;
        room.TemplateUpperBounds = roomTemplate.UpperBounds;
        room.ChildRoomIdList = new List<string>(roomNode.ChildRoomNodeIdList);
        room.Doorways = DeepCopyList(roomTemplate.GetDoorways());


        room.ParentRoomId = "";
        room.IsPreviouslyVisited = true;
        if(roomNode.ParentRoomNodeIdList.Count != 0)
        {
            room.ParentRoomId = roomNode.ParentRoomNodeIdList[0];
            room.IsPreviouslyVisited = false;
        }
        return room;
    }

    /// <summary>
    /// Create a deep copy of the list
    /// </summary>
    private List<T> DeepCopyList<T>(List<T> originalList) where T: IDeepClonable<T>
    {
        List<T> newList = new();
        foreach(T item in originalList)
        {
            newList.Add(item.DeepClone());
        }
        return newList;
    }

    private void ClearDungeon()
    {
        if(DungeonBuilderRoomDictionary.Count <= 0) return;
        foreach(KeyValuePair<string, Room> keyValue in DungeonBuilderRoomDictionary)
        {
            Room room = keyValue.Value;
            if(room.InstantiatedRoom == null) continue;
            Destroy(room.InstantiatedRoom.gameObject);
        }
        DungeonBuilderRoomDictionary.Clear();
    }

    private RoomNodeGraphSO GetRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if(roomNodeGraphList.Count == 0) return null;
        if(roomNodeGraphList.Count == 1) return roomNodeGraphList.First();
        return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
    }

    /// <summary>
    /// Get room template by id, if not exist = return null
    /// </summary>
    private RoomTemplateSO GetRoomTemplateById(string roomTemplateId)
    {
        if(RoomTemplateDictionary.TryGetValue(roomTemplateId, out var roomTemplateSO))
        {
            return roomTemplateSO;
        }
        return null;
    }

    /// <summary>
    /// Get room by id, if not exist = return true
    /// </summary>
    private Room GetRoomById(string roomId)
    {
        if(DungeonBuilderRoomDictionary.TryGetValue(roomId, out var room))
        {
            return room;
        }
        return null;
    }

    private void LoadRoomTemplatesIntoDictionary()
    {
        RoomTemplateDictionary.Clear();

        foreach (var roomTemplate in _roomTemplateList)
        {
            if(!RoomTemplateDictionary.ContainsKey(roomTemplate.GuId))
            {
                RoomTemplateDictionary.Add(roomTemplate.GuId, roomTemplate);
            }
            else
            {
                Debug.LogWarning("Duplicate room room template in " + _roomTemplateList);
            }
        }
    }
}
