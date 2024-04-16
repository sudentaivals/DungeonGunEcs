using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

public class SoundSystem : IEcsInitSystem, IEcsDestroySystem
{
    private GameObject _soundContainer;
    private List<AudioSource> _audioSources = new List<AudioSource>();
    private readonly int _baseNumberOfSounds = 15;
    private EcsPool<TransformComponent> _transformPool;

    private readonly float _minDistance = 1f;

    private readonly float _maxDistance = 10f;

    public void Destroy(IEcsSystems systems)
    {
        EcsEventBus.Unsubscribe(GameplayEventType.OnSfxVolumeChanged, OnSoundVolumeChanged);
        EcsEventBus.Unsubscribe(GameplayEventType.PlaySound, PlaySound);
    }

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _transformPool = world.GetPool<TransformComponent>();

        _soundContainer = new GameObject("SoundContainer");
        _soundContainer.transform.position = Vector3.zero;

        _audioSources = new List<AudioSource>();
        for (int i = 0; i < _baseNumberOfSounds; i++)
        {
            AddAudioSource();
        }
        foreach(var aS in _audioSources)
        {
            aS.volume = PlayerPrefsController.SfxVolume;
        }
        EcsEventBus.Subscribe(GameplayEventType.OnSfxVolumeChanged, OnSoundVolumeChanged);
        EcsEventBus.Subscribe(GameplayEventType.PlaySound, PlaySound);
    }

    private AudioSource AddAudioSource()
    {
        var gameObject = new GameObject();
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.parent = _soundContainer.transform;
        var aS = gameObject.AddComponent<AudioSource>();
        aS.playOnAwake = false;
        _audioSources.Add(aS);
        return aS;
    }

    private void OnSoundVolumeChanged(int entity, EventArgs args)
    {
        foreach (var aS in _audioSources)
        {
            aS.volume = 1f;
        }
    }

    private void PlaySound(int entity, EventArgs args)
    {
        var sfxArgs = args as PlaySoundEventArgs;
        if(sfxArgs.Sfx == null) return;
        var availableSource = _audioSources.FirstOrDefault(source => !source.isPlaying);
        var position = Vector3.zero;
        if(entity == -1) position = Camera.main.transform.position;
        else
        {
            if(_transformPool.Has(entity))
            {
                ref var transformComponent = ref _transformPool.Get(entity);
                position = transformComponent.Transform.position;
            }
        }
        if(availableSource == null) availableSource = AddAudioSource();
        availableSource.clip = sfxArgs.Sfx;
        availableSource.volume = sfxArgs.Volume * PlayerPrefsController.SfxVolume;
        availableSource.transform.position = position;

        availableSource.minDistance = _minDistance;
        availableSource.maxDistance = _maxDistance;
        availableSource.Play();
    }
}
