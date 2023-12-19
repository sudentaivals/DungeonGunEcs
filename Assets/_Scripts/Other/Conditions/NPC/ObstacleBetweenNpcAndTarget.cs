using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Conditions/NPC/Obstacle between sender and target")]
public class ObstacleBetweenNpcAndTarget : BaseGameCondition
{
    [SerializeField] bool _isObstacleExists;
    [SerializeField] LayerMask _obstacleLayer;

    private RaycastHit2D[] _obstacleColliders = new RaycastHit2D[1];

    private ContactFilter2D _filter;

    private void Awake()
    {
        _filter.SetLayerMask(_obstacleLayer);
        _filter.useTriggers = false;
        _filter.useDepth = false;
        _filter.useLayerMask = true;
        _filter.useNormalAngle = false;
    }
    public override bool CheckCondition(int senderEntity, int? takerEntity)
    {
        var npcTargetPool = EcsStart.World.GetPool<NpcTargetComponent>();
        ref var npcTargetComp = ref npcTargetPool.Get(senderEntity);
        var transformPool = EcsStart.World.GetPool<TransformComponent>();
        ref var npcTransform = ref transformPool.Get(senderEntity);
        ref var targetTransform = ref transformPool.Get(npcTargetComp.TargetEntity);
        var origin = npcTransform.Transform.position;
        var direction = (targetTransform.Transform.position - origin).normalized;
        var magnitude = (targetTransform.Transform.position - origin).magnitude;
        int raycast = Physics2D.RaycastNonAlloc(origin, direction, _obstacleColliders, magnitude, _obstacleLayer);
        //int raycast = Physics2D.Raycast(origin, direction, _filter, _obstacleColliders, magnitude);
        //var raycast = Physics2D.Raycast(origin, direction, magnitude, _obstacleLayer);
        if(raycast == 0)
        {
            if(_isObstacleExists) return false;
            return true;
        }
        else
        {
            if(_isObstacleExists) return true;
            return false;
        }


    }
}
