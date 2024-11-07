using System;
using System.Collections;
using CustomAstar;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [SerializeField] private GameObject _nsDoorPrefab;
    [SerializeField] private GameObject _ewDoorPrefab;
    [SerializeField] private  Transform _doorHolder;
    [SerializeField] private PathfindingGrid _pathfindGrid;
    [SerializeField] private float _activateGridTimer = 1.5f;
    [HideInInspector] public Room Room;
    [HideInInspector] public Grid Grid;
    [HideInInspector] public Tilemap GroundTilemap;
    [HideInInspector] public Tilemap FrontTilemap;
    [HideInInspector] public Tilemap Decoration1Tilemap;
    [HideInInspector] public Tilemap Decoration2Tilemap;
    [HideInInspector] public Tilemap CollisionTilemap;
    [HideInInspector] public Tilemap CollisionAllowProjectileTilemap;
    [HideInInspector] public Tilemap MinimapTilemap;
    [HideInInspector] public Bounds RoomColliderBounds;
    private bool _isInitialized = false;

    #region Tags

    private const string GROUND_TAG = "TGround";
    private const string FRONT_TAG = "TFront";
    private const string DECORATION1_TAG = "TDecoration1";
    private const string DECORATION2_TAG = "TDecoration2";
    private const string COLLISION_TAG = "TCollision";
    private const string COLLISION_ALLOW_PROJECTILE_TAG = "TCollisionAllowP";
    private const string MINIMAP_TAG = "TMinimap";

    #endregion

    private BoxCollider2D _boxCollider2D;

    private void Awake() 
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        RoomColliderBounds = _boxCollider2D.bounds;
    }

    /// <summary>
    /// Initialise the instantiated room
    /// </summary>
    public void Initialise(GameObject roomGameObject)
    {
        if(_isInitialized) return;
        _isInitialized = true;
        PopulateTilemapMemberVariables(roomGameObject);
        BlockUnusedDoorways();
        SpawnDoors();
        DisableCollisionTilemapRenderer();
    }

    private void SpawnDoors()
    {
        if(Room == null) return;
        if(Room.RoomNodeType.IsCorridor) return;
        if(Room.RoomNodeType.IsCorridorEW) return;
        if(Room.RoomNodeType.IsCorridorNS) return;
        foreach (Doorway doorway in Room.Doorways)
        {
            if(!doorway.IsConnected) continue;
            Vector3 pos = new Vector3(doorway.Position.x, doorway.Position.y, 0) + gameObject.transform.position;
            if(doorway.Orientaion == Orientation.North || doorway.Orientaion == Orientation.South) Instantiate(_nsDoorPrefab, pos, Quaternion.identity, _doorHolder);
            if(doorway.Orientaion == Orientation.East || doorway.Orientaion == Orientation.West) Instantiate(_ewDoorPrefab, pos, Quaternion.identity, _doorHolder);
        }
    }

    private void Start() 
    {
        Initialise(gameObject);
        if(_pathfindGrid == null) return;
        StartCoroutine(ActivateGridWithTimer(_activateGridTimer));
    }

    private IEnumerator ActivateGridWithTimer(float time)
    {
        yield return HelperUtilities.GetWait(time);
        _pathfindGrid.ActivateGrid();
    }

    private void BlockUnusedDoorways()
    {
        if(Room == null) return;
        foreach (Doorway doorway in Room.Doorways)
        {
            if(doorway.IsConnected) continue;
            if(CollisionTilemap != null) BlockDoorwayOnTilemapLevel(CollisionTilemap, doorway);
            if(MinimapTilemap != null) BlockDoorwayOnTilemapLevel(MinimapTilemap, doorway);
            if(CollisionAllowProjectileTilemap != null) BlockDoorwayOnTilemapLevel(CollisionAllowProjectileTilemap, doorway);
            if(GroundTilemap != null) BlockDoorwayOnTilemapLevel(GroundTilemap, doorway);
            if(FrontTilemap != null) BlockDoorwayOnTilemapLevel(FrontTilemap, doorway);
            if(Decoration1Tilemap != null) BlockDoorwayOnTilemapLevel(Decoration1Tilemap, doorway);
            if(Decoration2Tilemap != null) BlockDoorwayOnTilemapLevel(Decoration2Tilemap, doorway);
        }
    }

    private void BlockDoorwayOnTilemapLevel(Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.Orientaion)
        {
            case Orientation.East:
            case Orientation.West:
                BlockDoorwayVertically(tilemap, doorway);
                break;
            case Orientation.North:
            case Orientation.South:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;
            case Orientation.None:
                break;
        }
    }

    /// <summary>
    /// For north and south doorweays
    /// </summary>
    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.DoorwayStartCopyPosition;
        for (int x = 0; x < doorway.DoorwayCopyWidth; x++)
        {
            for (int y = 0; y < doorway.DoorwayCopyHeight; y++)
            {
                //get tile rotation
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + x, startPosition.y - y, 0));
                //copy tile
                var tileCopy = tilemap.GetTile(new Vector3Int(startPosition.x + x, startPosition.y - y, 0));
                var tileReplacePosition = new Vector3Int(startPosition.x + x + 1, startPosition.y - y, 0);
                tilemap.SetTile(tileReplacePosition, tileCopy);
                tilemap.SetTransformMatrix(tileReplacePosition, transformMatrix);
                tilemap.RefreshAllTiles();
            }
        }
    }

    /// <summary>
    /// For east and west doorweays
    /// </summary>
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.DoorwayStartCopyPosition;
        for (int yPos = 0; yPos < doorway.DoorwayCopyHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.DoorwayCopyWidth; xPos++)
            {
                //get tile rotation
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + yPos, startPosition.y - xPos, 0));
                //copy tile
                var tileCopy = tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
                var tileReplacePosition = new Vector3Int(startPosition.x + xPos, startPosition.y - yPos - 1, 0);
                tilemap.SetTile(tileReplacePosition, tileCopy);
                tilemap.SetTransformMatrix(tileReplacePosition, transformMatrix);
            }
        }
    }

    /// <summary>
    /// Disable the collision tilemap renderer, it should show collision only in editor
    /// </summary>
    private void DisableCollisionTilemapRenderer()
    {
        CollisionTilemap.GetComponent<TilemapRenderer>().enabled = false;

        CollisionAllowProjectileTilemap.GetComponent<TilemapRenderer>().enabled = false;
    }

    /// <summary>
    /// Populate tilemap and grid variables
    /// </summary>
    private void PopulateTilemapMemberVariables(GameObject roomGameObject)
    {
        Grid = roomGameObject.GetComponentInChildren<Grid>();

        Tilemap[] tilemaps = roomGameObject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if(tilemap.gameObject.CompareTag(GROUND_TAG)) GroundTilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(FRONT_TAG)) FrontTilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(DECORATION1_TAG)) Decoration1Tilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(DECORATION2_TAG)) Decoration2Tilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(COLLISION_TAG)) CollisionTilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(COLLISION_ALLOW_PROJECTILE_TAG)) CollisionAllowProjectileTilemap = tilemap;
            else if (tilemap.gameObject.CompareTag(MINIMAP_TAG)) MinimapTilemap = tilemap; 
        }
    }
}
