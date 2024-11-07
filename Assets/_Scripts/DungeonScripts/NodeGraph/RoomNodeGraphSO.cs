using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "My Assets/Dungeon/RoomNodeGraph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO RoomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> RoomNodeList = new();
    [HideInInspector] public Dictionary<string, RoomNodeSO> RoomNodeDictionary = new();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }


    /// <summary>
    /// Load room node dictionary from room node list
    /// </summary>
    private void LoadRoomNodeDictionary()
    {
       RoomNodeDictionary.Clear();

       foreach (RoomNodeSO node in RoomNodeList)
       {
           RoomNodeDictionary[node.Id] = node;
       }
    }

    /// <summary>
    /// Get room node by type
    /// </summary>
    public RoomNodeSO GetRoomNodeByType(RoomNodeTypeSO roomNodeType)
    {
        foreach (RoomNodeSO node in RoomNodeList)
        {
            if(node.RoomNodeType == roomNodeType) return node;
        }
        return null;
    }

    public RoomNodeSO GetRoomNodeById(string id)
    {
        if(RoomNodeDictionary.TryGetValue(id, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }

    /// <summary>
    /// Get child room nodes
    /// </summary>
    public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoomNode)
    {
        foreach (string childId in parentRoomNode.ChildRoomNodeIdList)
        {
            yield return GetRoomNodeById(childId);
        }
    }

    #region Editor code
#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO RoomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 LinePosition;

    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }
    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        RoomNodeToDrawLineFrom = node;
        LinePosition = position;
    }

#endif
    #endregion
}
