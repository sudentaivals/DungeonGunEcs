using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Shaders and materials/Set outline thicknessAction")]
public class SetOutlineThicknessAction : GameAction
{
    [SerializeField] private float _thickness;
    public override void Action(int senderEntity, int? takerEntity)
    {
       var args = EventArgsObjectPool.GetArgs<SetOutlineThicknessEventArgs>();
       args.Thickness = _thickness;
       EcsEventBus.Publish(GameplayEventType.SetOutlineThickness, senderEntity, args);
    }

    private void OnValidate()
    {
        if(_thickness < 0) _thickness = 0;
    }
}
