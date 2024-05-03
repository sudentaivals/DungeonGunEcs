using System.Collections;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle _roomNodeStyle;
    private GUIStyle _selectedRoomNodeStyle;
    private static RoomNodeGraphSO _currentRoomNodeGraph;
    private RoomNodeTypeListSO _roomNodeTypeList;

    private RoomNodeSO _currentSelectedNode = null;

    private Vector2 _graphOffset;
    private Vector2 _graphDrag;

    //node layout values
    private readonly float _nodeWidth = 160f;
    private readonly float _nodeHeight = 75f;
    private readonly int _nodePadding = 25;
    private readonly int _nodeBorder = 12;

    private readonly Vector2 _firstNodeOffset = new Vector2(100f, 200f);
    //connecting line variables
    private Color _connectiveLineColor = Color.white;
    private float _connectiveLineWidth = 6f;

    private float _coonectionLineArrowSize = 15f;
    //background grid variables
    private readonly float _gridLarge = 100f;
    private readonly float _gridlargeLineWidth = 0.2f;
    private readonly Color _gridLargeColor = Color.gray;
    private readonly float _gridLargeOpacity = 0.3f;
    private readonly float _gridSmall = 25f;
    private readonly float _gridSmallLineWidth = 0.2f;
    private readonly Color _gridSmallColor = Color.gray;
    private readonly float _gridSmallOpacity = 0.2f;

    [MenuItem("Room node graph editor", menuItem = "Window/Dungeon Editor/Room node graph editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room node graph editor");
    }

    /// <summary>
    /// Open the room node graph editor if a room node graph scriptable object asset is double clicked
    /// </summary>
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceId, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceId) as RoomNodeGraphSO;
        if(roomNodeGraph != null)
        {
            OpenWindow();
            _currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += InspectorSelectionChanged;

        _roomNodeStyle = new GUIStyle();
        _roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        _roomNodeStyle.normal.textColor = Color.white;
        _roomNodeStyle.padding = new RectOffset(_nodePadding, _nodePadding, _nodePadding, _nodePadding);
        _roomNodeStyle.border = new RectOffset(_nodeBorder, _nodeBorder, _nodeBorder, _nodeBorder);

        //define selected room node style
        _selectedRoomNodeStyle = new GUIStyle();
        _selectedRoomNodeStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        _selectedRoomNodeStyle.normal.textColor = Color.white;
        _selectedRoomNodeStyle.padding = new RectOffset(_nodePadding, _nodePadding, _nodePadding, _nodePadding);
        _selectedRoomNodeStyle.border = new RectOffset(_nodeBorder, _nodeBorder, _nodeBorder, _nodeBorder);



        _roomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= InspectorSelectionChanged;
    }

    private void InspectorSelectionChanged()
    {
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;
        if(roomNodeGraph != null)
        {
            _currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }

    /// <summary>
    /// Draw editor GUI
    /// </summary>
    private void OnGUI()
    {
        //if a SO of type RoomNodeGraphSO has been selected then process
        if(_currentRoomNodeGraph != null)
        {
            //draw background grid
            DrawBackgroundGrid(_gridSmall, _gridSmallLineWidth, _gridSmallColor, _gridSmallOpacity);
            DrawBackgroundGrid(_gridLarge, _gridlargeLineWidth, _gridLargeColor, _gridLargeOpacity);
            //draw dragged line
            DrawDraggedLine();
            //process events
            ProcessEvents(Event.current);
            //draw room node connections
            DrawRoomConnections();
            //draw room node
            DrawRoomNodes();
        }

        if(!GUI.changed) return;
        Repaint();
    }

    private void DrawBackgroundGrid(float gridSize, float lineSize, Color lineColor, float gridOpacity)
    {
        int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
        int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);
        Handles.color = new Color(lineColor.r, lineColor.g, lineColor.b, gridOpacity);
        _graphOffset += _graphDrag * 0.5f;

        Vector3 gridOffset = new Vector3(_graphOffset.x % gridSize, _graphOffset.y % gridSize, 0);
        for (int i = 0; i < verticalLineCount; i++)
        {
            Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset, lineSize);
        }
        for (int i = 0; i < horizontalLineCount; i++)
        {
            Handles.DrawLine(new Vector3(-gridSize, gridSize * i, 0) + gridOffset, new Vector3(position.width + gridSize, gridSize * i, 0f) + gridOffset, lineSize);
        }
        Handles.color = Color.white;
    }

    /// <summary>
    /// draw room node connections
    /// </summary>
    private void DrawRoomConnections()
    {
        foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            foreach (string childNodeId in roomNode.ChildRoomNodeIdList)
            {
                if (!_currentRoomNodeGraph.RoomNodeDictionary.ContainsKey(childNodeId)) continue;
                DrawConnectionLine(roomNode, _currentRoomNodeGraph.RoomNodeDictionary[childNodeId]);
                GUI.changed = true;
            }
        }
    }

    /// <summary>
    /// Draw a line from parent node to child node
    /// </summary>
    private void DrawConnectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
    {
        Vector2 startPosition = parentRoomNode.Rect.center;
        Vector2 endPosition  = childRoomNode.Rect.center;
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, _connectiveLineColor, null, _connectiveLineWidth);
        //draw direction arrows
        Vector2 direction = (endPosition - startPosition).normalized;
        Vector2 midPosition = (startPosition + endPosition) / 2f;
        Vector2 perpendecularVector = Vector2.Perpendicular(direction);

        Vector2 arrowHeadPoint = midPosition + direction * _coonectionLineArrowSize;
        Vector2 arrowTailPoint1 = midPosition + perpendecularVector * _coonectionLineArrowSize;
        Vector2 arrowTailPoint2 = midPosition - perpendecularVector * _coonectionLineArrowSize;
        Handles.DrawBezier(arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, _connectiveLineColor, null, _connectiveLineWidth);
        Handles.DrawBezier(arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, _connectiveLineColor, null, _connectiveLineWidth);
        
        GUI.changed = true;
    }

    /// <summary>
    /// Draw dragged line from one node mouse position (==line position)
    /// </summary>
    private void DrawDraggedLine()
    {
        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom == null) return;
        if(_currentRoomNodeGraph.LinePosition == Vector2.zero) return;
        Vector3 startPosition = _currentRoomNodeGraph.RoomNodeToDrawLineFrom.Rect.center;
        Vector3 endPosition = _currentRoomNodeGraph.LinePosition;
        Vector3 startTangent = startPosition;
        Vector3 endTangent = endPosition;
        Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, _connectiveLineColor, null, _connectiveLineWidth);
    }

    private void ProcessEvents(Event currentEvent)
    {
        _graphDrag = Vector2.zero;
        if(_currentSelectedNode == null || _currentSelectedNode.IsLeftCLickDragging == false)
        {
            _currentSelectedNode = IsMouseOverRoomNode(currentEvent);
        }
        if(_currentSelectedNode == null || _currentRoomNodeGraph.RoomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }
        else
        {
            _currentSelectedNode.ProcessEvents(currentEvent);
        }
    }

    /// <summary>
    /// Check to see if mouse is over a room node
    /// </summary>
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            if(!roomNode.Rect.Contains(currentEvent.mousePosition)) continue;
            return roomNode;
        }
        return null;
    }

    /// <summary>
    /// Process room node graph events
    /// </summary>
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch(currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse down events
    /// </summary>
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //RMB events
        if(currentEvent.button == 1)
        {
            ProcessRightMouseUpEvents(currentEvent);
        }
    }

    /// <summary>
    /// Process RMB up event
    /// </summary>
    private void ProcessRightMouseUpEvents(Event currentEvent)
    {
        //Connect two nodes
        ConnectNodes(currentEvent);
        //clear line if nodes not connected
        ClearLineDrag(currentEvent);
    }

    /// <summary>
    /// Connect two nodes if mouse if overlaying
    /// </summary>
    private void ConnectNodes(Event currentEvent)
    {
        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom == null) return;
        var roomNode = IsMouseOverRoomNode(currentEvent);
        if(roomNode == null) return;
        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom == roomNode) return;

        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom.AddRoomNodeIdToChildList(roomNode.Id))
        {
            roomNode.AddRoomNodeIdToParentList(_currentRoomNodeGraph.RoomNodeToDrawLineFrom.Id);
        }

        _currentRoomNodeGraph.RoomNodeToDrawLineFrom = null;
        GUI.changed = true;
    }

    /// <summary>
    /// Clear line on RMB is up and no connection between nods
    /// </summary>
    private void ClearLineDrag(Event currentEvent)
    {
        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom == null) return;

        _currentRoomNodeGraph.RoomNodeToDrawLineFrom = null;
        _currentRoomNodeGraph.LinePosition = Vector2.zero;
        GUI.changed = true;
    }

    /// <summary>
    /// Process mouse drag
    /// </summary>
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if(currentEvent.button == 1)
        {
            ProcessRightMouseDragEvents(currentEvent);
        }
        if(currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvents(currentEvent);
        }
    }

    /// <summary>
    /// Process left click mouse drag
    /// </summary>
    private void ProcessLeftMouseDragEvents(Event currentEvent)
    {
        _graphDrag = currentEvent.delta;
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            roomNode.DragNode(currentEvent.delta);
        }
        GUI.changed = true;
    }

    /// <summary>
    /// Process right click mouse drag
    /// </summary>
    private void ProcessRightMouseDragEvents(Event currentEvent)
    {
        if(_currentRoomNodeGraph.RoomNodeToDrawLineFrom == null) return;
        DragConnectingLine(currentEvent);
        GUI.changed = true;
    }

    /// <summary>
    /// Drag connecting line from room node
    /// </summary>
    private void DragConnectingLine(Event currentEvent)
    {
        _currentRoomNodeGraph.LinePosition += currentEvent.delta;
    }

    /// <summary>
    /// Process mouse down events on the room node graph (not on the node)
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if(currentEvent.button == 0)
        {
            ProcessLeftMouseDownEvents(currentEvent);
        }
        //process right-click mouse button
        if(currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }

    /// <summary>
    /// LMB down button events
    /// </summary>
    private void ProcessLeftMouseDownEvents(Event currentEvent)
    {
        ClearLineDrag(currentEvent);
        ClearAllSelectedRoomNodes();
    }

    /// <summary>
    /// Clear all selected room nodes
    /// </summary>
    private void ClearAllSelectedRoomNodes()
    {
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            if(roomNode.IsSelected)
            {
                roomNode.IsSelected = false;
                GUI.changed = true;
            }
        }
    }

    /// <summary>
    /// Show the context menu
    /// </summary>
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Create room node"), false, CreateRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select all nodes"), false, SelectAllNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Delete selected room nodes links"), false, DeleteSelectedRoomNodesLinks);
        menu.AddItem(new GUIContent("Delete selected room nodes"), false, DeleteSelectedRoomNodes);
        menu.ShowAsContext();
    }

    /// <summary>
    /// Delete selected room nodes
    /// </summary>
    private void DeleteSelectedRoomNodes()
    {
        Queue<RoomNodeSO> roomNodeQueue = new Queue<RoomNodeSO>();
        foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            if (roomNode.RoomNodeType.IsEntrance) continue;
            if (!roomNode.IsSelected) continue;
            roomNodeQueue.Enqueue(roomNode);
            //clear child nodes
            foreach (string childNodeId in roomNode.ChildRoomNodeIdList)
            {
                var childNode = _currentRoomNodeGraph.GetRoomNodeById(childNodeId);
                if (childNode == null) continue;
                childNode.RemoveParentRoomNodeId(roomNode.Id);
            }
            //clear parent nodes
            foreach (string parentNodeId in roomNode.ParentRoomNodeIdList)
            {
                var parentNode = _currentRoomNodeGraph.GetRoomNodeById(parentNodeId);
                if (parentNode == null) continue;
                roomNode.RemoveChildRoomNodeId(roomNode.Id);
            }
        }
        while(roomNodeQueue.TryDequeue(out var roomNodeToDelete))
        {
            //remove from dictionary
            _currentRoomNodeGraph.RoomNodeDictionary.Remove(roomNodeToDelete.Id);
            //remove from list
            _currentRoomNodeGraph.RoomNodeList.Remove(roomNodeToDelete);

            DestroyImmediate(roomNodeToDelete, true);

            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// Delete links between selected room nodes
    /// </summary>
    private void DeleteSelectedRoomNodesLinks()
    {
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            if(!roomNode.IsSelected) continue;
            if(roomNode.ChildRoomNodeIdList.Count == 0) continue;
            for (int i = roomNode.ChildRoomNodeIdList.Count - 1; i >= 0 ; i--)
            {
                var childNode = _currentRoomNodeGraph.GetRoomNodeById(roomNode.ChildRoomNodeIdList[i]);
                if(childNode == null) continue;
                if(!childNode.IsSelected) continue;
                roomNode.RemoveChildRoomNodeId(childNode.Id);
                childNode.RemoveParentRoomNodeId(roomNode.Id);
            }
        }
        ClearAllSelectedRoomNodes();
    }

    /// <summary>
    /// Select all room nodes
    /// </summary>
    private void SelectAllNodes()
    {
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            roomNode.IsSelected = true;
        }
        GUI.changed = true;
    }

    /// <summary>
    /// Create a room node at the mouse position
    /// </summary>
    private void CreateRoomNode(object mousePosition)
    {
        if(_currentRoomNodeGraph.RoomNodeList.Count == 0)
        {
            Vector2 mousePos = (Vector2)mousePosition;
            CreateRoomNode(mousePos + _firstNodeOffset, _roomNodeTypeList.List.Find(a => a.IsEntrance));
        }
        CreateRoomNode(mousePosition, _roomNodeTypeList.List.Find(a => a.IsNone));
    }

    /// <summary>
    /// Create a room node at the mouse position - overloaded to pass in a room node type
    /// </summary>
    private void CreateRoomNode(object mousePosition, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePos = (Vector2)mousePosition;
        //create room node SO asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();
        //add room node to current room node graph room node list
        _currentRoomNodeGraph.RoomNodeList.Add(roomNode);

        //set roomNode values
        roomNode.Initialise(new Rect(mousePos, new Vector2(_nodeWidth, _nodeHeight)), _currentRoomNodeGraph, roomNodeType);

        //add rome node to room node graph SO asset database
        AssetDatabase.AddObjectToAsset(roomNode, _currentRoomNodeGraph);
        AssetDatabase.SaveAssets();

        _currentRoomNodeGraph.OnValidate();
    }

    /// <summary>
    /// Draw room nodes in the graph window
    /// </summary>
    private void DrawRoomNodes()
    {
        foreach(RoomNodeSO roomNode in _currentRoomNodeGraph.RoomNodeList)
        {
            if(roomNode.IsSelected) roomNode.Draw(_selectedRoomNodeStyle);
            else roomNode.Draw(_roomNodeStyle);
        }
        GUI.changed = true;
    }

}
