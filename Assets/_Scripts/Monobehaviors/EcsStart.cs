using Leopotam.EcsLite;
using Voody.UniLeo.Lite;
using UnityEngine;

public class EcsStart : MonoBehaviour
{
    public static EcsWorld World;
    EcsSystems _updateSystems;
    EcsSystems _fixedUpdateSystems;
    EcsSystems _lateUpdateSystems;

    EcsSystems _editorSystems;


    void OnEnable()
    {
        World = new EcsWorld();
        _fixedUpdateSystems = new EcsSystems(World);
        _lateUpdateSystems = new EcsSystems(World);
        _updateSystems = new EcsSystems(World);
        _editorSystems = new EcsSystems(World);
        _updateSystems.ConvertScene()
            #region EventsOnly
            .Add(new ForcedDirectionMovementSystem())
            .Add(new CameraGroupSystem())
            .Add(new NpcMovementSystem())
            .Add(new GlobalStatsSystem())
            .Add(new SoundSystem())
            .Add(new PhysicsSystem())
            .Add(new DungeonDoorSystem())
            .Add(new HealthSystem())
            .Add(new DealDamageSystem())
            #endregion
            .Add(new MouseFollowSystem())
            .Add(new ObjectPoolSystem())
            .Add(new PlayerInputSystem())
            .Add(new RotationSystem())
            .Add(new SkillSystem())
            .Add(new PlayerSkillControllerSystem())
            .Add(new PlayerUseObjectsSystem())
            .Add(new GunRotationSystem())
            //.Add(new PlayerStateChangeSystem())
            .Add(new NewAnimationSystem())
            .Add(new FloatingTextSystem())
            .Add(new StatusEffectSystem())
            .Add(new NpcSkillSelectionSystem())
            .Add(new MaterialSystem())
            .Add(new NpcBehaviorSystem())
            .Add(new CorpseStatsSystem())
            .Add(new SummonedObjectSystem())
            .Add(new StartEventsInvokerSystem())
            .Add(new UsableObjectSystem())
            .Add(new TimedActionSystem())
            .Init();
        _fixedUpdateSystems
            .Add(new NpcToTargetPathSystem())
            .Add(new NodeCheckerSystem())
            #region Directions
            .Add(new DirectionSystem())
            #endregion
            .Add(new MovementSystem()) //adding movement force to objects
            .Add(new FlipSystem()) //flip object if direction vector > 0
            .Add(new CombineMovementSystem()) //velocity = movement + push
            .Add(new PushRemoveSystem()) //remove push using "gravity"
            .Add(new TargetSystem())
            .Init();
        _lateUpdateSystems
            .Add(new UiHealthbarSystem())
            //.Add(new SummonedObjectSystem())
            .Add(new ProjectileStartupSystem())
            .Add(new BulletPathSystem())
            .Add(new DestroyAndSpawnEntitiesSystem())
            .Init();
#if UNITY_EDITOR
        _editorSystems
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            .Init();
#endif
    }


    void Update()
    {
        _updateSystems?.Run();
#if UNITY_EDITOR
        _editorSystems?.Run();
#endif
        
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void LateUpdate()
    {
        _lateUpdateSystems?.Run();
    }


    private void OnDestroy()
    {
        if (_updateSystems != null)
        {
            _updateSystems.Destroy();
            _updateSystems = null;
            _fixedUpdateSystems.Destroy();
            _fixedUpdateSystems = null;
            _lateUpdateSystems.Destroy();
            _lateUpdateSystems = null;
        }
#if UNITY_EDITOR
        if(_editorSystems != null)
        {
            _editorSystems.Destroy();
            _editorSystems = null;
        }
#endif
        if (World != null)
        {
            World.Destroy();
            World = null;
        }
    }
}
