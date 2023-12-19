using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] AnimancerComponent _animancerComponent;
    [SerializeField] AnimationClip _idleAnimation;

    private AnimancerState _state;

    private void Start()
    {
        _state = _animancerComponent.Play(_idleAnimation);
    }

    private void Update()
    {
        if(_state.NormalizedTime >= 1)
        {
            _state.NormalizedTime = 0f;
            _state.Events.OnEnd = OnEnd;
            OnEnd();
        }
    }
    private void OnEnd()
    {
        Debug.Log("Finish");
    }
}
