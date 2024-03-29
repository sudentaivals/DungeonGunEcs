using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

[CreateAssetMenu(menuName = "My Assets/Input/Player input reader")]
public class PlayerInputNewSystem : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;
    public event Action<bool> UseObjectEvent;
    private PlayerControls _playerControls;
    public Vector2 AimPosition {get; private set;}

    private void OnEnable()
    {
        if(_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.SetCallbacks(this);
        }    
        _playerControls.Enable();
    }
    public void OnAiming(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
        //movement.Normalize();
        MoveEvent?.Invoke(movement);
    }

    public void OnPrimaryfire(InputAction.CallbackContext context)
    {
        if(context.performed) PrimaryFireEvent?.Invoke(true);
        if(context.canceled) PrimaryFireEvent?.Invoke(false);
    }

    public void OnUseobject(InputAction.CallbackContext context)
    {
        if(context.performed) UseObjectEvent?.Invoke(true);
        if(context.canceled) UseObjectEvent?.Invoke(false);
    }

    public void OnRoll(InputAction.CallbackContext context)
    {

    }

    public void OnAlternativefire(InputAction.CallbackContext context)
    {

    }


}
