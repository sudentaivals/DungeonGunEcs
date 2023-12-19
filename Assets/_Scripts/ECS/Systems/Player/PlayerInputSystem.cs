using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter<PlayerInputComponent>().End();
        var playerInputPool = world.GetPool<PlayerInputComponent>();

        foreach (int entity in filter)
        {
            ref PlayerInputComponent playerInputComponent = ref playerInputPool.Get(entity);
            playerInputComponent.HorizontalMovement = Input.GetAxisRaw("Horizontal");
            playerInputComponent.VerticalMovement = Input.GetAxisRaw("Vertical");
            playerInputComponent.RollPressed = Input.GetButtonDown("Roll");
            playerInputComponent.IsShooting = Input.GetButtonDown("Fire1");
            playerInputComponent.IsAlternateShooting = Input.GetButtonDown("Fire2");
            playerInputComponent.IsAutomaticShooting = Input.GetButton("Fire1");
        }
    }
}
