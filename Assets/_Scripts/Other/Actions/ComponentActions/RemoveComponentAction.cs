using AYellowpaper;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Remove component")]
public class RemoveComponentAction : GameAction
{
    [SerializeField] bool _targetIsSender;
    [SerializeField] InterfaceReference<IRemoveComponent, ScriptableObject> _removeComponent;
    public override void Action(int senderEntity, int? takerEntity)
    {
        if(_targetIsSender) _removeComponent.Value.RemoveComponent(senderEntity);
        else
        {
            if(takerEntity == null) return;
            _removeComponent.Value.RemoveComponent(takerEntity.Value);
        }
    }
}
