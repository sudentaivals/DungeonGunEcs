using AYellowpaper;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Add push")]
public class AddPushAction : GameAction
{
    [SerializeField] float _pushPower;
    [SerializeField] InterfaceReference<IRotationType, ScriptableObject> _rotationType;
    [SerializeField] bool _isReversed;
    [SerializeField] bool _targetIsSender;
    public override void Action(int senderEntity, int? takerEntity)
    {
        if (!_targetIsSender && !takerEntity.HasValue) return;
        var rotation = _rotationType.Value.GetRotation(senderEntity, takerEntity);
        if(_isReversed) rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        var pushDirection = new Vector2
        (Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad), 
        Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad));
        int entity = _targetIsSender ? senderEntity : takerEntity.Value;
        var pushArgs = EventArgsObjectPool.GetArgs<AddPushEventArgs>();
        pushArgs.PushPower = _pushPower;
        pushArgs.Direction = pushDirection;
        EcsEventBus.Publish(GameplayEventType.AddPush, entity, pushArgs);



        // var statsPool = EcsStart.World.GetPool<StatsComponent>();
        // var invokerPool = EcsStart.World.GetPool<InvokerComponent>();
        // var transformPool = EcsStart.World.GetPool<TransformComponent>();
        // var physicalPool = EcsStart.World.GetPool<PhysicalBodyComponent>();
        // ref var stats = ref _targetIsSender ? ref statsPool.Get(senderEntity) : ref statsPool.Get(takerEntity.Value);
        // ref var physicalBody = ref _targetIsSender ? ref physicalPool.Get(senderEntity) : ref physicalPool.Get(takerEntity.Value);
        // Quaternion rotation = Quaternion.identity;
        // switch (_startPoint)
        // {
        //     case PushStartDirection.SenderDirection:
        //         rotation = transformPool.Get(senderEntity).Transform.rotation;
        //         if(transformPool.Get(senderEntity).Transform.localScale.x < 0)
        //         {
        //             rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        //         }
        //         break;
        //     case PushStartDirection.SenderInvokerDirection:
        //         Vector2 senderToSenderInvoker = (invokerPool.Get(senderEntity).InvokerPosition.position - transformPool.Get(senderEntity).Transform.position).normalized;
        //         var angle = Mathf.Rad2Deg * Mathf.Atan2(senderToSenderInvoker.y, senderToSenderInvoker.x);
        //         rotation = Quaternion.Euler(0, 0, angle);
        //         break;
        //     case PushStartDirection.TargetDirection:
        //         rotation = transformPool.Get(takerEntity.Value).Transform.rotation;
        //         if (transformPool.Get(takerEntity.Value).Transform.localScale.x < 0)
        //         {
        //             rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        //         }
        //         break;
        //     case PushStartDirection.FromSenderToTarget:
        //         break;
        //     case PushStartDirection.FromTargetToSender:
        //         break;
        //     default:
        //         break;
        // }

        // switch (_pushDirection)
        // {
        //     case PushDirectionSetting.Towards:
        //         break;
        //     case PushDirectionSetting.Backwards:
        //         rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        //         break;
        //     default:
        //         break;
        // }
        // Vector2 newPush = new Vector2(Mathf.Cos(rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(rotation.eulerAngles.z * Mathf.Deg2Rad)) * _pushPower * (1f - stats.PushResistance);

        // stats.Push = new Vector2(stats.Push.x + (Time.fixedDeltaTime * newPush.x) / physicalBody.RigidBody.mass,
        //                          stats.Push.y + (Time.fixedDeltaTime * newPush.y) / physicalBody.RigidBody.mass);
    }
}
