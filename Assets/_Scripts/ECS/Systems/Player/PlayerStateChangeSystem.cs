using Leopotam.EcsLite;
using UnityEngine;

public class PlayerStateChangeSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<PlayerStateComponent> _playerStatePool;
    private EcsPool<PhysicalBodyComponent> _physicalBodyPool;
    private EcsPool<PlayerInputComponent> _playerInputPool;

    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _playerStatePool = null;
        _physicalBodyPool = null;
        _playerInputPool = null;

    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerStateComponent>().End();
        _playerStatePool = world.GetPool<PlayerStateComponent>();
        _physicalBodyPool = world.GetPool<PhysicalBodyComponent>();
        _playerInputPool = world.GetPool<PlayerInputComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var playerState = ref _playerStatePool.Get(entity);
            ref var physicalBody = ref _physicalBodyPool.Get(entity);
            ref var playerInput = ref _playerInputPool.Get(entity);

            playerState.CurrentState = SelectNextState(ref playerState, new Vector2(playerInput.HorizontalMovement, playerInput.VerticalMovement));
        }
    }

    private string SelectNextState(ref PlayerStateComponent playerStateComponent, Vector2 velocity)
    {
        //die

        //roll

        //run
        if (velocity.magnitude > float.Epsilon)
        {
            return playerStateComponent.MoveState;
        }
        //run fast

        //idle
        return playerStateComponent.IdleState;
    }
}
