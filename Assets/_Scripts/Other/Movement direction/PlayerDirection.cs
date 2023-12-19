using Leopotam.EcsLite;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Movement direction/Player input")]
public class PlayerDirection : ScriptableObject, IMovementDirection
{
    private EcsPool<PlayerInputComponent> _playerInputPool;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    public Vector2 GetDirection(int sender)
    {
        if(_playerInputPool == null) _playerInputPool = EcsStart.World.GetPool<PlayerInputComponent>();
        if(_movementStatsPool == null) _movementStatsPool = EcsStart.World.GetPool<MovementStatsComponent>();

        if(!_playerInputPool.Has(sender)) return Vector2.zero;
        ref var movementStatsComponent = ref _movementStatsPool.Get(sender);
        ref var inputComponent = ref _playerInputPool.Get(sender);
        return new Vector2(inputComponent.HorizontalMovement, inputComponent.VerticalMovement).normalized;
    }
}

