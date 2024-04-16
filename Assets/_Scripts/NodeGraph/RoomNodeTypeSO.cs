using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "My Assets/Dungeon/RoomNodeType")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string RoomNodeTypeName;
    public bool DisplayInNodeGraphEditor = true;
    public bool IsCorridor;
    public bool IsCorridorNS;
    public bool IsCorridorEW;
    public bool IsEntrance;
    public bool IsBossRoom;
    public bool IsNone;

    #region Validation
    #if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(RoomNodeTypeName), RoomNodeTypeName);
    }
    #endif
    #endregion
}
