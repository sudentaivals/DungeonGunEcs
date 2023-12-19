using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDirectionSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<MovementStatsComponent> _movementStatsPool;
    private EcsPool<PlayerInputComponent> _playerInputPool;
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _movementStatsPool = null;
        _playerInputPool = null;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerInputComponent>().End();
        _movementStatsPool = world.GetPool<MovementStatsComponent>();
        _playerInputPool = world.GetPool<PlayerInputComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var input = ref _playerInputPool.Get(entity);
            ref var stats = ref _movementStatsPool.Get(entity);

            //shooting
            //movement
            stats.MovementDirection = stats.IsMovementAvailable ? new Vector2(input.HorizontalMovement, input.VerticalMovement).normalized : Vector2.zero;
            /*var targetSpeed = stats.MovementSpeed * new Vector2(input.HorizontalMovement, input.VerticalMovement).normalized;
            var runAccelAmount = (50f * stats.Acceleration) / stats.MovementSpeed;
            var runDeccelAmount = (50f * stats.Decceleration) / stats.MovementSpeed;
            float accelRate = Mathf.Abs(targetSpeed.magnitude) > 0.01f ? runAccelAmount : runDeccelAmount;
            var speedDiff = targetSpeed - stats.Movement;
            var movement = speedDiff * accelRate;
            stats.Movement = new Vector2(stats.Movement.x + (Time.fixedDeltaTime * movement.x) / movementComp.RigidBody.mass,
                                         stats.Movement.y + (Time.fixedDeltaTime * movement.y) / movementComp.RigidBody.mass);
            */
            //roll

        }
    }
}
