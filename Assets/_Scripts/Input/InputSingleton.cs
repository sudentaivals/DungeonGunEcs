using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSingleton : SingletonInstance<InputSingleton>
{
    public PlayerControls PlayerControls {get; private set;}
    public Vector2 MousePosition => PlayerControls.Player.Aiming.ReadValue<Vector2>();
    public override void Awake()
    {
        base.Awake();
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
    }
}
