using UnityEngine;
using Voody.UniLeo.Lite;

public class ParticleCallback : MonoBehaviour
{
    [SerializeField] GameAction _actionOnParticleStopped;
    private void OnParticleSystemStopped()
    {
        var convertToEntity = gameObject.GetComponent<ConvertToEntity>();
        if(!convertToEntity.TryGetEntity().HasValue) return;
        int entity = convertToEntity.TryGetEntity().Value;
        _actionOnParticleStopped?.Action(entity, null);
    }
}
