using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomTemplate", menuName = "My Assets/Dungeon/Room template")]
public class RoomTemplateSO : ScriptableObject
{
    [HideInInspector]public string GuId;
    #region  Room prefab
    [Space(10)]
    [Header("Room Prefab")]
    [Tooltip("Room template prefab that contains all tilemaps and environment")]
    #endregion
    public GameObject RoomPrefab;
    [HideInInspector] public GameObject PreviousRoomPrefab;
    #region  Room configuration
    [Space(10)]
    [Header("Room configuration")]
    [Tooltip("The room node type SO.")]
    #endregion
    public RoomNodeTypeSO RoomNodeType;

    public Vector2Int LowerBounds;

    public Vector2Int UpperBounds;

    [SerializeField] List<Doorway> _doorwayList;

    public List<Vector2Int> SpawnPositionsArray;

    public List<Doorway> GetDoorways()
    {
        return _doorwayList;
    }

    #region  Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(GuId == "" || PreviousRoomPrefab != RoomPrefab)
        {
            GuId = GUID.Generate().ToString();
            PreviousRoomPrefab = RoomPrefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(_doorwayList), _doorwayList);

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(SpawnPositionsArray), SpawnPositionsArray);
    }
#endif
    #endregion
}
