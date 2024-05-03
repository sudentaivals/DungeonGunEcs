using UnityEngine;

[System.Serializable]
public class Doorway
{
    public Vector2Int Position;
    public Orientation Orientaion;
    public GameObject DoorPrefab;
    #region Header
    [Header("Top left position to start copying from")]
    #endregion
    public Vector2Int DoorwayStartCopyPosition;
    #region Header
    [Header("Width of doorways to copy")]
    #endregion
    public int DoorwayCopyWidth;
    #region Header
    [Header("Height of doorways to copy")]
    #endregion
    public int DoorwayCopyHeight;
    [HideInInspector] public bool IsConnected = false;
    [HideInInspector] public bool IsUnavailable = false;

}
