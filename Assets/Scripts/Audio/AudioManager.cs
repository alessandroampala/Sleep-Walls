using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    private float playerMovementPitch = 1f;
    private bool disabled = false;

    private void Awake()
    {
        instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        if (!s.source.enabled) return;

        //switch(s.type)
        //{
        //    case SoundType.PlaceTile:
        //    case SoundType.Error:
        //    case SoundType.Button:
        //        s.source.pitch = UnityEngine.Random.Range(.7f, 1.3f);
        //        break;
        //    case SoundType.PlayerMovement:
        //        //s.source.pitch = UnityEngine.Random.Range(.7f, 1.3f);
        //        s.source.pitch = instance.playerMovementPitch;
        //        instance.playerMovementPitch += 0.1f;
        //        break;
        //    case SoundType.Trophy:
        //        s.source.pitch = Mathf.Lerp(1, instance.playerMovementPitch, 0.25f);
        //        break;
        //}

        switch (s.name)
        {
            case "Destroy":
                s.source.pitch = UnityEngine.Random.Range(.7f, 1.3f);
                break;
        }

        s.source.Play();
    }


}
