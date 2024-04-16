using UnityEngine;

public class PlayerPrefsController
{
    const string SFX_VOLUME_KEY = "sfx_volume";
    const string MUSIC_VOLUME_KEY = "music_volume";
    const float MIN_VOLUME = 0f;
    const float MAX_VOLUME = 1f;
    const float DEFAULT_VOLUME = 1f;

    public static float SfxVolume
    {
        get
        {
            if(!PlayerPrefs.HasKey(SFX_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(SFX_VOLUME_KEY, DEFAULT_VOLUME);
            }
            return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
        }
    }

    public static float MusicVolume
    {
        get
        {
            if(!PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
            {
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
            }
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        }
    }

    public static void SetSfxVolume(float volume)
    {
        var checkedValue = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, checkedValue);
        var args = EventArgsObjectPool.GetArgs<SetSfxVolumeEventArgs>();
        args.Volume = checkedValue;
        EcsEventBus.Publish(GameplayEventType.OnSfxVolumeChanged, -1, args);
    }

    public static void SetMusicVolume(float volume)
    {
        var checkedValue = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, checkedValue);
        EcsEventBus.Publish(GameplayEventType.OnMusicVolumeChanged, -1, null);
    }
}
