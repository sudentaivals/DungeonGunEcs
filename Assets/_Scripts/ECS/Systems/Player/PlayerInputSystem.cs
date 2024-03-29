using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _filter;
    private EcsPool<PlayerInputComponent> _playerInputPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<PlayerInputComponent>().End();
        _playerInputPool = world.GetPool<PlayerInputComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref PlayerInputComponent playerInputComponent = ref _playerInputPool.Get(entity);
            var playerControls = InputSingleton.Instance.PlayerControls;
            if(playerControls == null) continue;
            playerInputComponent.HorizontalMovement = playerControls.Player.Move.ReadValue<Vector2>().x;
            playerInputComponent.VerticalMovement = playerControls.Player.Move.ReadValue<Vector2>().y;
            playerInputComponent.RollPressed = playerControls.Player.Roll.WasPressedThisFrame();
            playerInputComponent.IsShooting = playerControls.Player.Primaryfire.WasPressedThisFrame();
            playerInputComponent.IsAlternateShooting = playerControls.Player.Alternativefire.WasPressedThisFrame();
            playerInputComponent.IsAutomaticShooting = playerControls.Player.Primaryfire.IsPressed();
            //playerInputComponent.UsableObjectIsSwapped = Input.GetButtonDown("Fire3");
            playerInputComponent.UseObject = playerControls.Player.Useobject.WasPressedThisFrame();
        }
    }
}
