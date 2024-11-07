using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Actions/Take object from pool")]
public class TakeObjectFromPoolAction : GameAction
{
    //[SerializeField] int _poolId;
    [SerializeField] GameObject _prefab;

    [SerializeField] InterfaceReference<IPositionType, ScriptableObject> _posType;
    [SerializeField] InterfaceReference<IRotationType, ScriptableObject> _rotType;

    [SerializeField] List<InterfaceReference<IMbHelperStatsSetter, ScriptableObject>> _statsSetters;

    [SerializeField] float _rotationDelta;

    [SerializeField] float _distanceDelta;
    [SerializeField] private Vector2 _spawnOffset = Vector2.zero;


    public override void Action(int senderEntity, int? takerEntity, ConditionAndActionArgs conditionAndActionArgs = null)
    {
        var args = EventArgsObjectPool.GetArgs<TakeObjectFromPoolEventArgs>();
        args.ObjectToSpawn = _prefab;
        args.Position = GetPosition(senderEntity);
        args.Rotation = GetRotation(senderEntity);
        args.StatsSetters = _statsSetters.Select(x => x.Value).ToList();
        EcsEventBus.Publish(GameplayEventType.TakeObjectFromPool, senderEntity, args);
    }

    private Vector3 GetPosition(int senderEntity)
    {
        var delta = Random.insideUnitCircle * _distanceDelta;
        return _posType.Value.GetPosition(senderEntity, null) + delta + _spawnOffset;
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
