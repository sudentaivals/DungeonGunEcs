using System.Collections;
using System.Collections.Generic;
using AYellowpaper;
using UnityEngine;
using Voody.UniLeo.Lite;

[CreateAssetMenu(menuName = "My Assets/Actions/Spawn object")]
public class SpawnObjectAction : GameAction
{
    [SerializeField] GameObject _prefabToSpawn;
    [SerializeField] float _rotationDelta;

    [SerializeField] InterfaceReference<IPositionType, ScriptableObject> _posType;
    [SerializeField] InterfaceReference<IRotationType, ScriptableObject> _rotType;

    public override void Action(int senderEntity, int? takerEntity)
    {
        Vector3 position = GetPosition(senderEntity);
        Quaternion rotation = GetRotation(senderEntity);
        var spawnArgs = EventArgsObjectPool.GetArgs<SpawnEntityEventArgs>();
        spawnArgs.PrefabToSpawn = _prefabToSpawn;
        spawnArgs.Position = position;
        spawnArgs.Rotation = rotation;
        EcsEventBus.Publish(GameplayEventType.SpawnObject, senderEntity, spawnArgs);
    }

    private Vector3 GetPosition(int senderEntity)
    {
        return _posType.Value.GetPosition(senderEntity, null);
    }

    private Quaternion GetRotation(int senderEntity)
    {
        var rotation = _rotType.Value.GetRotation(senderEntity, null);
        //set rotation delta
        var rotationDelta = UnityEngine.Random.Range(-_rotationDelta / 2f, _rotationDelta / 2f);
        rotation *= Quaternion.AngleAxis(rotationDelta, Vector3.forward);
        return rotation;

    }
}

