using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private Camera _mainCamera;
    private EcsFilter _filter;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<GunRotationComponent> _gunRotationPool;
    public void Init(IEcsSystems systems)
    {
        _mainCamera = Camera.main;

        var world = systems.GetWorld();
        _filter = world.Filter<GunRotationComponent>().End();
        _transformPool = world.GetPool<TransformComponent>();
        _gunRotationPool = world.GetPool<GunRotationComponent>();

        EcsEventBus.Subscribe(GameplayEventType.UnparentGun, UnparentGun);
    }

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.UnparentGun, UnparentGun);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var gunRotation = ref _gunRotationPool.Get(entity);
            ref var transform = ref _transformPool.Get(entity);


            var testVector = new Vector2(1f, 0f);
            var mousePos = InputSingleton.Instance.MousePosition;
            var mouseToWorldPos = _mainCamera.ScreenToWorldPoint(mousePos);
            mouseToWorldPos.z = 0;

            var playerMouseVector = mouseToWorldPos - transform.Transform.position;
            var dotProduct = Vector2.Dot(playerMouseVector.normalized, testVector);
            var angle = Mathf.Atan2(playerMouseVector.y, playerMouseVector.x);
            if (dotProduct > 0)
            {
                gunRotation.Gun.position = transform.Transform.position + (playerMouseVector.normalized * gunRotation.GunDistance);
                gunRotation.Gun.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle);
                gunRotation.Gun.localScale = new Vector3(Mathf.Sign(gunRotation.Gun.localScale.x) < 0 ? gunRotation.Gun.localScale.x * -1 : gunRotation.Gun.localScale.x, gunRotation.Gun.localScale.y, gunRotation.Gun.localScale.z);
            }
            else if(dotProduct < 0)
            {
                gunRotation.Gun.position = transform.Transform.position + (playerMouseVector.normalized * gunRotation.GunDistance);
                gunRotation.Gun.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle + 180f);
                gunRotation.Gun.localScale = new Vector3(Mathf.Sign(gunRotation.Gun.localScale.x) > 0 ? gunRotation.Gun.localScale.x * -1 : gunRotation.Gun.localScale.x, gunRotation.Gun.localScale.y, gunRotation.Gun.localScale.z);

            }
        }
    }

    private void UnparentGun(int sender, EventArgs args)
    {
        if(_gunRotationPool == null) return;
        if(!_gunRotationPool.Has(sender)) return;

        ref var gunRotation = ref _gunRotationPool.Get(sender);
        gunRotation.Gun.parent = null;
    }
}
