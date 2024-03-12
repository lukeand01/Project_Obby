using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [Separator("REFERENCES")]
    [SerializeField] Transform soundContainer;

    [Separator("VOLUME")]
    [SerializeField] float sfxVolume;
    [SerializeField] float backgroundMusicVolumeInitial;

    float backgroundMusicVolumeCurrent;

    //also control music here.
    //control sfx as well.


    AudioSource backgroundMusicAudioSource;

    //who will be giving the music.

    private void Awake()
    {
        backgroundMusicAudioSource = GetComponent<AudioSource>();

        if(backgroundMusicAudioSource == null )
        {
            backgroundMusicAudioSource = gameObject.AddComponent<AudioSource>();
        }

        backgroundMusicAudioSource.loop = true;

        SetBackgroundVolume(backgroundMusicVolumeInitial);

    }


    public void SetBackgroundVolume( float volume)
    {
        backgroundMusicAudioSource.volume = volume;
        backgroundMusicVolumeCurrent = volume;
    }

    public void ChangeBackgroundMusic(AudioClip clip, bool isForced = false)
    {
        if (isForced)
        {
            backgroundMusicAudioSource.Stop();
            backgroundMusicAudioSource.clip = clip;
            backgroundMusicAudioSource.Play();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ChangeBackgroundMusicProcess(clip));
        }

        
    }

    public void StopBGMusic()
    {
        backgroundMusicAudioSource.Stop();
    }
    public void StartBGMusic()
    {
        backgroundMusicAudioSource.Play();
    }

    IEnumerator ChangeBackgroundMusicProcess(AudioClip clip)
    {
        //we lower the current to then bring the next one.

        while (backgroundMusicAudioSource.volume > 0)
        {
            backgroundMusicAudioSource.volume -= 1;
            yield return new WaitForSeconds(0.1f);
        }

        backgroundMusicAudioSource.Stop();
        backgroundMusicAudioSource.clip = clip;
        backgroundMusicAudioSource.Play();

        while(backgroundMusicAudioSource.volume < backgroundMusicVolumeCurrent)
        {
            backgroundMusicAudioSource.volume += 1;
            yield return new WaitForSeconds(0.1f);
        }

    }


    public void CreateSFX(AudioClip clip)
    {
        GameObject newObject = new GameObject();
        newObject.transform.SetParent(soundContainer);
        AudioSource source = newObject.AddComponent<AudioSource>();
        newObject.AddComponent<DestroySelf>().SetUpDestroy(clip.length + 0.1f);
        source.clip = clip;
        source.volume = sfxVolume;
        source.Play();
    }
}
