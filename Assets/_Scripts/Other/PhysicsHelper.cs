using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Voody.UniLeo.Lite;

public static class PhysicsHelper
{
    public static int? GetClosestEnemyEntity(int senderEntity, Vector3 position, float radius)
    {
        return 1;
    }
    public static int? GetClosestFriendlyEntity(int senderEntity, Vector3 position, float radius)
    {
        return 1;
    }

    public static List<int> GetAllEntitiesInRadius(Vector3 position, float radius)
    {
        var entitesInRange = Physics2D.OverlapCircleAll(position, radius)
            .Select(a => a.GetComponent<ConvertToEntity>())
            .Where(a => a != null)
            .Where(a => a.TryGetEntity() != null)
            .Select(a => a.TryGetEntity().Value)
            .ToList();
        return entitesInRange;
    }
    public static List<int> GetAllEntitiesInRadius(int senderEntity, Vector3 position, float radius)
    {
        var entitesInRange = Physics2D.OverlapCircleAll(position, radius)
            .Select(a => a.GetComponent<ConvertToEntity>())
            .Where(a => a != null)
            .Where(a => a.TryGetEntity() != null)
            .Select(a => a.TryGetEntity().Value)
            .Where(a => a != senderEntity)
            .ToList();
        return entitesInRange;
    }


}
