using Leopotam.EcsLite;

public class CorpseStatsSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    private EcsFilter _filter;
    private EcsPool<TransformComponent> _transformPool;
    private EcsPool<MaterialComponent> _materialPool;
    public void Destroy(IEcsSystems systems)
    {
        return;
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _transformPool = world.GetPool<TransformComponent>();
        _materialPool = world.GetPool<MaterialComponent>();
        _filter = world.Filter<CorpseTag>()
                       .Exc<PooledObjectTag>()
                       .End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (int entity in _filter)
        {
            ref var transformComp = ref _transformPool.Get(entity);
            ref var materialComp = ref _materialPool.Get(entity);
            if(transformComp.Transform.TryGetComponent<CorpseMbHelper>(out var corpse))
            {
                if(corpse.CorpseDataIsTransfered) continue;
                corpse.CorpseDataIsTransfered = true;
                materialComp.Renderer.color = corpse.Color;
                materialComp.Renderer.sprite = corpse.Sprite;
                transformComp.Transform.localScale = corpse.Scale;
            }
        }
    }
}
