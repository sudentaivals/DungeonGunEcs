using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector2 _xRange;
    [SerializeField] private Vector2 _yRange;

    [SerializeField] private int _numberOfSpawns;

    [SerializeField] private GameObject _objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _numberOfSpawns; i++)
        {
            var spawnXPos = transform.position.x + UnityEngine.Random.Range(_xRange.x, _xRange.y);
            var spawnYPos = transform.position.y + UnityEngine.Random.Range(_yRange.x, _yRange.y);
            var spawnArgs = EventArgsObjectPool.GetArgs<SpawnEntityEventArgs>();
            spawnArgs.PrefabToSpawn = _objectToSpawn;
            spawnArgs.Position = new Vector3(spawnXPos, spawnYPos, 0);
            spawnArgs.Rotation = Quaternion.identity;
            EcsEventBus.Publish(GameplayEventType.SpawnObject, -1, spawnArgs);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_xRange.y - _xRange.x, _yRange.y - _yRange.x, 0));
    }

}
