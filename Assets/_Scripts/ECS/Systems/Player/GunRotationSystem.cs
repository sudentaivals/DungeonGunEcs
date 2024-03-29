using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotationSystem : IEcsRunSystem, IEcsInitSystem
{
    private Camera _mainCamera;
    public void Init(IEcsSystems systems)
    {
        _mainCamera = Camera.main;

        var world = systems.GetWorld();

        var filter = world.Filter<GunRotationComponent>().End();
        var gunRotationPool = world.GetPool<GunRotationComponent>();

        foreach (int entity in filter)
        {
            ref var gunRotation = ref gunRotationPool.Get(entity);
            gunRotation.Gun.parent = null;
        }

    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        var filter = world.Filter<GunRotationComponent>().End();
        var transformPool = world.GetPool<TransformComponent>();
        var gunRotationPool = world.GetPool<GunRotationComponent>();

        foreach (int entity in filter)
        {
            ref var gunRotation = ref gunRotationPool.Get(entity);
            ref var transform = ref transformPool.Get(entity);


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
}
