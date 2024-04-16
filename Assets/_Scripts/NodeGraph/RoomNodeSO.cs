using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string Id;
    [HideInInspector] public List<string> ParentRoomNodeIdList = new();
    [HideInInspector] public List<string> ChildRoomNodeIdList = new();
    [HideInInspector] public RoomNodeGraphSO RoomNodeGraph;
    public RoomNodeTypeSO RoomNodeType;
    [HideInInspector] RoomNodeTypeListSO RoomNodeTypeList => GameResources.Instance.RoomNodeTypeList;

    #region Editor code
#if UNITY_EDITOR
    [HideInInspector] public Rect Rect;
    [HideInInspector] public bool IsLeftCLickDragging = false;
    [HideInInspector] public bool IsSelected = false;
    /// <summary>
    /// Initialise code
    /// </summary>
    public void Initialise(Rect rect, RoomNodeGraphSO roomNodeGraph, RoomNodeTypeSO roomNodeType)
    {
        Rect = rect;
        Id = Guid.NewGuid().ToString();
        name = "RoomNode";
        RoomNodeGraph = roomNodeGraph;
        RoomNodeType = roomNodeType;

        //load room node type list
        //RoomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
    }

    /// <summary>
    /// Draw node with the node style
    /// </summary>
    public void Draw(GUIStyle nodeStyle)
    {
        //Draw node box using Begin area
        GUILayout.BeginArea(Rect, nodeStyle);

        //start Region to detect popup slection changes
        EditorGUI.BeginChangeCheck();

        //display only label if node is parent or entrance
        if(ParentRoomNodeIdList.Count > 0 || RoomNodeType.IsEntrance)
        {
            EditorGUILayout.LabelField(RoomNodeType.RoomNodeTypeName);
        }
        else
        {
            //display popup using the RoomNodeType name values that can be selected from (default to currently selected)
            int selected = RoomNodeTypeList.List.FindIndex(a => a == RoomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());
            RoomNodeType = RoomNodeTypeList.List[selection];

            bool isSelectionInvalid = false;
            if(RoomNodeTypeList.List[selected].IsCorridor && !RoomNodeTypeList.List[selection].IsCorridor) isSelectionInvalid = true;
            if(!RoomNodeTypeList.List[selected].IsCorridor && RoomNodeTypeList.List[selection].IsCorridor) isSelectionInvalid = true;
            if(!RoomNodeTypeList.List[selected].IsBossRoom && RoomNodeTypeList.List[selection].IsBossRoom) isSelectionInvalid = true;
            if(isSelectionInvalid)
            {
                if(ChildRoomNodeIdList.Count > 0)
                {
                    for (int i = ChildRoomNodeIdList.Count - 1; i >= 0 ; i--)
                    {   
                        var childNode = RoomNodeGraph.GetRoomNodeById(ChildRoomNodeIdList[i]);
                        if(childNode == null) continue;
                        RemoveChildRoomNodeId(childNode.Id);
                        childNode.RemoveParentRoomNodeId(Id);
                    }
                }
            }
        }
        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(this);
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Populate a string array with the room node types that can be selected
    /// </summary>
    private string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomNodeTypes = new string[RoomNodeTypeList.List.Count];
        for (int i = 0; i < roomNodeTypes.Length; i++)
        {
            if(!RoomNodeTypeList.List[i].DisplayInNodeGraphEditor) continue;
            roomNodeTypes[i] = RoomNodeTypeList.List[i].RoomNodeTypeName;
        }
        return roomNodeTypes;
    }

    /// <summary>
    /// Process events for the node
    /// </summary>
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse drag
    /// </summary>
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    /// <summary>
    /// Process left mouse drag
    /// </summary>
    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        IsLeftCLickDragging = true;
        DragNode(currentEvent.delta);

        GUI.changed = true;
    }

    /// <summary>
    /// Drag node logic
    /// </summary>
    public void DragNode(Vector2 delta)
    {
        Rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    /// <summary>
    /// Process mouse up event
    /// </summary>
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        
        if(currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    /// <summary>
    /// Process left click up event
    /// </summary>
    private void ProcessLeftClickUpEvent()
    {
        if(IsLeftCLickDragging) IsLeftCLickDragging = false;
    }

    /// <summary>
    /// Process mouse down event
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
        if(currentEvent.button == 1)
        {
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    /// <summary>
    /// Process mouse right click down
    /// </summary>
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        RoomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }

    /// <summary>
    /// Process left click down
    /// </summary>
    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;
        if(IsSelected)
        {
            IsSelected = false;
        }
        else
        {
            IsSelected = true;
        }
    }
    /// <summary>
    /// Add parentId to list of parent nodes
    /// </summary>
    public bool AddRoomNodeIdToParentList(string parentId)
    {
        if(ParentRoomNodeIdList.Contains(parentId)) return false;
        ParentRoomNodeIdList.Add(parentId);
        return true;
    }

    /// <summary>
    /// remove node from child list (if this id is in the list)
    /// </summary>
    public bool RemoveChildRoomNodeId(string childId)
    {
        if(!ChildRoomNodeIdList.Contains(childId)) return false;
        ChildRoomNodeIdList.Remove(childId);
        return true;
    }

    /// <summary>
    /// Add childId to list of child nodes
    /// </summary>
    public bool AddRoomNodeIdToChildList(string childId)
    {
        if(!IsChildRoomValid(childId)) return false;
        ChildRoomNodeIdList.Add(childId);
        return true;
    }

    /// <summary>
    /// Check if childId is valid. Checking dublicates, self, parent, number of child corridors
    /// </summary>
    private bool IsChildRoomValid(string childId)
    {
        //is child id valid
        if(RoomNodeGraph.GetRoomNodeById(childId) == null) return false;
        //check for none type of room
        if(RoomNodeGraph.GetRoomNodeById(childId).RoomNodeType.IsNone) return false;
        //check for boss room - only one boss room allowed
        bool isConnectedBossRoomAlready = false;
        foreach(RoomNodeSO roomNode in RoomNodeGraph.RoomNodeList)
        {
            if(roomNode.RoomNodeType.IsBossRoom && roomNode.ParentRoomNodeIdList.Count > 0) isConnectedBossRoomAlready = true;
        }
        if(RoomNodeGraph.GetRoomNodeById(childId).RoomNodeType.IsBossRoom && isConnectedBossRoomAlready) return false;
        //check for entrance, entrance cant be a child of another node
        if(RoomNodeGraph.GetRoomNodeById(childId).RoomNodeType.IsEntrance) return false;
        //do not allow duplicate child nodes
        if(ChildRoomNodeIdList.Contains(childId)) return false;
        //do not allow self as child node
        if(childId == Id) return false;
        //do not allow parent as child node
        if(ParentRoomNodeIdList.Contains(childId)) return false;
        //if child already have parent
        if(RoomNodeGraph.GetRoomNodeById(childId).ParentRoomNodeIdList.Count > 0) return false;
        //check if child is corridor
        bool isChildIdCorridor = RoomNodeGraph.GetRoomNodeById(childId).RoomNodeType.IsCorridor;
        //if this node is corridor and child node is corridor return false
        if(isChildIdCorridor && RoomNodeType.IsCorridor) return false;
        //if this node NOT a corridor and child node is NOT a corridor return false
        if(!isChildIdCorridor && !RoomNodeType.IsCorridor) return false;
        //do not alow more than N corridor nodes
        if(isChildIdCorridor)
        {
            int currentNumberOfCorridorsInChildList = ChildRoomNodeIdList
            .Select(nodeId => RoomNodeGraph.RoomNodeDictionary[nodeId])
            .Where(node => node.RoomNodeType.IsCorridor)
            .Count();
            if(currentNumberOfCorridorsInChildList >= Settings.MaxChildCorridors) return false;
        }
        //if adding room to corridor, check that this corridor node dont have a room added
        if(!isChildIdCorridor && ChildRoomNodeIdList.Count > 0) return false;
        return true;
    }

    /// <summary>
    /// Remove parentId from list of parent nodes (if node is in the list)
    /// </summary>
    public bool RemoveParentRoomNodeId(string parentId)
    {
        if(!ParentRoomNodeIdList.Contains(parentId)) return false;
        ParentRoomNodeIdList.Remove(parentId);
        return true;
    }
#endif
    #endregion
}
