using Leopotam.EcsLite;
using UnityEngine;
[CreateAssetMenu(menuName = "My Assets/Mb stats setter/Set corpse stats")]
public class SetCorpseStats : ScriptableObject, IMbHelperStatsSetter 
{
    private EcsPool<MaterialComponent> _materialComponentPool = null;

    private EcsPool<TransformComponent> _transformPool = null;

    public void SetStats(GameObject takerGameObject, int senderEntity)
    {
        if(_materialComponentPool == null) _materialComponentPool = EcsStart.World.GetPool<MaterialComponent>();
        if(_transformPool == null) _transformPool = EcsStart.World.GetPool<TransformComponent>();
        if(takerGameObject.TryGetComponent<CorpseMbHelper>(out var corpseMbHelper))
        {
            if(corpseMbHelper.CorpseDataIsTransfered) return;

            ref var senderTransformComponent = ref _transformPool.Get(senderEntity);
            ref var senderMaterialComponent = ref _materialComponentPool.Get(senderEntity);

            corpseMbHelper.Color = Color.white;
            corpseMbHelper.Sprite = senderMaterialComponent.Renderer.sprite;
            corpseMbHelper.Scale = senderTransformComponent.Transform.localScale;
        }
    }
}
