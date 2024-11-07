using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources _instance;

    public static GameResources Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameResources>("Prefabs/System/GameResources");
            }
            return _instance;
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #region Tooltip
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    #endregion
    #endregion
    public RoomNodeTypeListSO RoomNodeTypeList;

    #region Header GAME STATES
    [Space(10)]
    [Header("GAME STATES")]

    #region Tooltip
    [Tooltip("Populate with the GameStateListSO")]
    #endregion
    #endregion
    public GameStateListSO GameStateList;

    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]

    #region Tooltip
    [Tooltip("Dimmed material")]
    #endregion
    public Material DimmedMaterial;

    #region Tooltip
    [Tooltip("Variable material")]
    #endregion
    public Material VariableMaterial;
    
    #endregion

}
