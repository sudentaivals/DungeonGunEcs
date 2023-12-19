using AYellowpaper;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Actions/Add component action")]
public class AddComponentAction : GameAction
{
    [SerializeField] bool _targetIsSender;
    [SerializeField] InterfaceReference<IAddComponent, ScriptableObject> _addComponent;

    public override void Action(int senderEntity, int? takerEntity)
    {
        if(_targetIsSender) _addComponent.Value.AddComponent(senderEntity);
        else
        {
            if(takerEntity == null) return;
            _addComponent.Value.AddComponent(takerEntity.Value);
        }
    }
}
