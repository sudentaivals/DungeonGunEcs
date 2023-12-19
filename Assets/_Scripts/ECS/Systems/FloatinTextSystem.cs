using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<FloatingTextComponent> _floatingTextPool;
    private EcsPool<TransformComponent> _transformPool;

    private string FloatingTextPath => "Prefabs/FloatingText/Floating text";
    public void Destroy(IEcsSystems systems)
    {
        _filter = null;
        _floatingTextPool = null;
        _transformPool = null;
        EcsEventBus.Unsubscribe(GameplayEventType.CreateFloatingText, CreateFloatingText);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<FloatingTextComponent>().Exc<PooledObjectTag>().End();
        _floatingTextPool = world.GetPool<FloatingTextComponent>();
        _transformPool = world.GetPool<TransformComponent>();
        EcsEventBus.Subscribe(GameplayEventType.CreateFloatingText, CreateFloatingText);

    }

    private void CreateFloatingText(int senderEntity, EventArgs args)
    {
        var floatTextArgs = args as CreateFloatingTextEventArgs;
        var gameObject = GameObject.Instantiate(Resources.Load<GameObject>(FloatingTextPath));
        var floatingTextHandler = gameObject.GetComponent<FloatingTextHandler>();

        floatingTextHandler.SpawnPosition = _transformPool.Get(senderEntity).Transform.position;
        floatingTextHandler.Text = floatTextArgs.Text;
        floatingTextHandler.Color = floatTextArgs.Color;
        floatingTextHandler.Direction = floatTextArgs.Direction.normalized;
        floatingTextHandler.Velocity = floatTextArgs.Velocity;
        floatingTextHandler.Lifetime = floatTextArgs.Lifetime;

    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            //activate
            ActivateText(entity);
            //add timer
            ChangeTimer(entity);
            //move
            MoveText(entity);
            //change opacity
            ChangeTextOpacity(entity);
            //destroy
            DestroyText(entity);
        }
    }

    private void ChangeTextOpacity(int entity)
    {
        ref var floatingTextComponent = ref _floatingTextPool.Get(entity);
        var tempColor = floatingTextComponent.Text.color;
        var timerRelation = floatingTextComponent.CurrentLifetime / floatingTextComponent.MaxLifetime;
        tempColor.a = Mathf.Lerp(0f, 1f, timerRelation);
        floatingTextComponent.Text.color = tempColor;
    }

    private void DestroyText(int entity)
    {
        ref var floatingTextComponent = ref _floatingTextPool.Get(entity);
        if(floatingTextComponent.CurrentLifetime <= 0)
        {
            EcsEventBus.Publish(GameplayEventType.DestroyObject, entity, null);
        }
    }

    private void ChangeTimer(int entity)
    {
        ref var floatingTextComponent = ref _floatingTextPool.Get(entity);
        floatingTextComponent.CurrentLifetime -= Time.deltaTime;
    }

    private void MoveText(int entity)
    {
        ref var floatingTextComponent = ref _floatingTextPool.Get(entity);
        floatingTextComponent.Transform.Translate(floatingTextComponent.Direction * floatingTextComponent.Velocity * Time.deltaTime, Space.World);
    }

    private void ActivateText(int entity)
    {
        ref var floatingTextComponent = ref _floatingTextPool.Get(entity);
        if (!floatingTextComponent.IsActivated)
        {
            var floatingTextHelper = floatingTextComponent.Text.GetComponent<FloatingTextHandler>();
            floatingTextComponent.Text.text = floatingTextHelper.Text;
            floatingTextComponent.Transform.position = floatingTextHelper.SpawnPosition;
            floatingTextComponent.Text.color = floatingTextHelper.Color;
            floatingTextComponent.Direction = floatingTextHelper.Direction.normalized;
            floatingTextComponent.CurrentLifetime = floatingTextHelper.Lifetime;
            floatingTextComponent.MaxLifetime = floatingTextHelper.Lifetime;
            floatingTextComponent.Velocity = floatingTextHelper.Velocity;
            floatingTextComponent.IsActivated = true;
            GameObject.Destroy(floatingTextHelper);
        }
    }
}
