using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Animation state")]
public class BaseAnimationState : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] AnimationClip _animation;
    [SerializeField] BaseGameCondition _animationStartCondition;
    [SerializeField] GameAction _actionOnAnimationEnd;
    [SerializeField] GameAction _actionOnAnimationStart;
    [SerializeField] int _priority;
    public int Id => _id;

    public AnimationClip Animation => _animation;
    public int Priority => _priority;

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

}
