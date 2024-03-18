using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<MaterialComponent> _materialPool;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.AddColor, AddColor);
        EcsEventBus.Unsubscribe(GameplayEventType.RemoveColor, RemoveColor);
        EcsEventBus.Unsubscribe(GameplayEventType.ChangeSprite, ChangeSprite);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<MaterialComponent>().End();
        _materialPool = world.GetPool<MaterialComponent>();
        EcsEventBus.Subscribe(GameplayEventType.AddColor, AddColor);
        EcsEventBus.Subscribe(GameplayEventType.RemoveColor, RemoveColor);
        EcsEventBus.Subscribe(GameplayEventType.ChangeSprite, ChangeSprite);
    }

    private void ChangeSprite(int sender, EventArgs args)
    {
        var spriteChangeArgs = args as ChangeSpriteEventArgs;
        ref var materialComp = ref _materialPool.Get(sender);
        materialComp.Renderer.sprite = spriteChangeArgs.Sprite;

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var materialComp = ref _materialPool.Get(entity);
            if (materialComp.Colors == null) materialComp.Colors = new Stack<Color>();
            //MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            var color = materialComp.Colors.Count == 0 ? Color.white : materialComp.Colors.Peek();
            //mpb.SetColor("_Color", color);
            materialComp.Renderer.color = color;

        }
    }

    private void AddColor(int senderEntity, EventArgs args)
    {
        var colorArgs = args as AddColorEventArgs;
        if (_materialPool.Has(colorArgs.TakerEntity))
        {
            ref var materialComp = ref _materialPool.Get(colorArgs.TakerEntity);
            materialComp.Colors.Push(colorArgs.Color);
        }
    }

    private void RemoveColor(int senderEntity, EventArgs args)
    {
        var colorArgs = args as RemoveColorEventArgs;
        if (_materialPool.Has(colorArgs.TakerEntity))
        {
            ref var materialComp = ref _materialPool.Get(colorArgs.TakerEntity);
            var colorList = materialComp.Colors.ToList();
            colorList.Remove(colorArgs.Color);
            materialComp.Colors = new Stack<Color>(colorList);
        }

    }
}
