using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "My Assets/Dungeon/Dungeon level")]
public class DungeonLevelSO : ScriptableObject
{
    [Space(10)]
    [Header("Basic level details")]
    public string LevelName;

    [Space(10)]
    [Header("Room templates for level")]
    public List<RoomTemplateSO> RoomTemplates;

    [Space(10)]
    [Header("Room node graph for level")]
    public List<RoomNodeGraphSO> RoomNodeGraphList;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(LevelName), LevelName);
        if(HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomTemplates), RoomTemplates)) return;
        if(HelperUtilities.ValidateCheckEnumerableValues(this, nameof(RoomNodeGraphList), RoomNodeGraphList)) return;

        bool isEntrance = false;
        bool isSNCorridor = false;
        bool isEWCorridor = false;

        //check templates
        foreach (RoomTemplateSO roomTemplate in RoomTemplates)
        {
            if(roomTemplate == null) return;
            if(roomTemplate.RoomNodeType.IsCorridorEW) isEWCorridor = true;

            if(roomTemplate.RoomNodeType.IsCorridorNS) isSNCorridor = true;

            if(roomTemplate.RoomNodeType.IsEntrance) isEntrance = true;
        }

        if(isEWCorridor == false)
        {
            Debug.Log("In" + this.name.ToString() + " there is no EW Corridor");
        }
        if(isSNCorridor == false)
        {
            Debug.Log("In" + this.name.ToString() + " there is no NS Corridor");
        }
        if(isEntrance == false)
        {
            Debug.Log("In" + this.name.ToString() + " there is no entrance");
        }

        //check node graphs
        foreach (RoomNodeGraphSO roomNodeGraph in RoomNodeGraphList)
        {
            if(roomNodeGraph == null) return;
            foreach (var roomNodeSo in roomNodeGraph.RoomNodeList)
            {
                if(roomNodeSo == null) continue;
                if(roomNodeSo.RoomNodeType.IsEntrance || roomNodeSo.RoomNodeType.IsCorridorEW || roomNodeSo.RoomNodeType.IsCorridorNS ||
                    roomNodeSo.RoomNodeType.IsCorridor || roomNodeSo.RoomNodeType.IsNone)
                    continue;
                bool isRoomNodeTypeFound = false;
                foreach (RoomTemplateSO roomTemplate in RoomTemplates)
                {
                    if(roomTemplate == null) continue;
                    if(roomTemplate.RoomNodeType == roomNodeSo.RoomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if(!isRoomNodeTypeFound) Debug.Log("In" + this.name.ToString() + " : No room template " + roomNodeSo.RoomNodeType.name + " found for node graph " + 
                    roomNodeGraph.name.ToString());
            }
        }
    }
#endif
    #endregion
}
