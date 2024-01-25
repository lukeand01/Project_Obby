using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXUnit : MonoBehaviour
{
    AudioSource source;

    float current;
    float total;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetUp(AudioClip clip, float volume = 1)
    {
        gameObject.AddComponent<AudioSource>();
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.Play();

        total = clip.length + 0.1f;
        current = 0;
    }

    private void Update()
    {
        if(current > total)
        {
            Destroy(gameObject);
        }
        else
        {
            current += Time.deltaTime;
        }
    }

}
