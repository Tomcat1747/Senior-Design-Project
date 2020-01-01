using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : AudioManagerBase
{ // We'll extend the functionality of the original to include ambient noise
    public static List<AmbientSFX> allAmbientSFX = new List<AmbientSFX>();

    public void AddAmbientSFX(AudioClip SFX, float maxVolume, float pitch = 1f, float startingVolume = 0f)
    {
        if (SFX != null)
        {
            bool isPlaying = false;
            for (int i = 0; i < allAmbientSFX.Count; i++)
            {
                AmbientSFX sfx = allAmbientSFX[i];
                if (sfx.clip == SFX) // if the ambient sound is already here
                {
                    isPlaying = true;
                    break;
                }
            }
            if (!isPlaying)
            {
                new AmbientSFX(SFX, maxVolume, pitch, startingVolume);
            }
        }
    }

    public void RemoveAmbientSFX(AudioClip SFX)
    {
        for (int i = 0; i < allAmbientSFX.Count; i++)
        {
            AmbientSFX sfx = allAmbientSFX[i];
            if (sfx.clip == SFX)
            {
                allAmbientSFX[i].DestroySFX();
                break;
            }
        }
    }

    public void ClearAllAmbientSFX()
    {
        for (int i = 0; i < allAmbientSFX.Count; i++)
        {
            allAmbientSFX[i].DestroySFX();
        }
    }

    [System.Serializable]
    public class AmbientSFX : SONG
    {
        public AmbientSFX(AudioClip clip, float _maxVolume, float pitch, float startingVolume) : base(clip, _maxVolume, pitch, startingVolume, true, true)
        {
            source = AudioManager.CreateNewSource(string.Format("AmbientSFX [{0}]", clip.name));
            source.clip = clip;
            maxVolume = _maxVolume;
            source.pitch = pitch;
            source.volume = startingVolume;
            source.loop = true;
            source.Play();

            AudioManager.allAmbientSFX.Add(this);
        }

        public override void DestroySFX()
        {
            AudioManager.allAmbientSFX.Remove(this);
            DestroyImmediate(source.gameObject);
        }
    }
}
