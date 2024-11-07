using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Animation state")]
public class BaseAnimationState : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] private string _name;
    [SerializeField] AnimationClip _animation;
    [SerializeField] BaseGameCondition _animationStartCondition;
    [Header("Actions")]
    [Tooltip("Actions on exit, triggers when change animation, even if animation not ended.")]
    [SerializeField] GameAction _actionOnAnimationExit;
    [Tooltip("End of animation actions, if looped all actions will perform at last keyframe. If not looped, this actions will trigger only once.")]
    [SerializeField] GameAction _actionOnAnimationEnd;
    [Tooltip("Start of animation actions, if looped all actions will perform at first keyframe. If not looped, this actions will trigger only once.")]
    [SerializeField] GameAction _actionOnAnimationStart;
    [Tooltip("Higher priority plays first")]
    [SerializeField] int _priority;
    [SerializeField] bool _isRepeatable;
    public int Id => _id;
    public bool IsRepeatable => _isRepeatable;
    public AnimationClip Animation => _animation;
    public int Priority => _priority;

    public string Name => _name;

    public bool CheckStartCondition(int sender)
    {
        if(_animationStartCondition == null) return true;
        return _animationStartCondition.CheckCondition(sender, null);
    }

    public void OnAnimationStart(int sender)
    {
        _actionOnAnimationStart?.Action(sender, null);
    }

    public void OnAnimationEnd(int sender)
    {
        _actionOnAnimationEnd?.Action(sender, null);
    }

    public void OnAnimationExit(int sender)
    {
        _actionOnAnimationExit?.Action(sender, null);
    }

}
