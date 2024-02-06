using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoSounds : MonoBehaviour {

    public AudioClip[] effects, loops;
    public Text effectsPlay, loopsPlay, effectsCurrent, loopsCurrent;

    int effectsIndex = 0;
    int loopsIndex = 0;

    bool effectsPlaying = false;
    bool loopsPlaying = false;

    float timestamp;

	// Use this for initialization
	void Start () {
        timestamp = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (effectsPlaying)
        {
            if (Time.timeSinceLevelLoad > timestamp + effects[effectsIndex].length)
            {
                effectsIndex++;
                if (effectsIndex >= effects.Length)
                {
                    effectsIndex = 0;
                }
                this.GetComponent<AudioSource>().PlayOneShot(effects[effectsIndex]);
                timestamp = Time.timeSinceLevelLoad;
                effectsCurrent.text = effects[effectsIndex].name;
            }
        }
        else if (loopsPlaying)
        {
            if (Time.timeSinceLevelLoad > timestamp + loops[loopsIndex].length)
            {
                loopsIndex++;
                if (loopsIndex >= loops.Length)
                {
                    loopsIndex = 0;
                }
                this.GetComponent<AudioSource>().PlayOneShot(loops[loopsIndex]);
                timestamp = Time.timeSinceLevelLoad;
                loopsCurrent.text = loops[loopsIndex].name;
            }
        }
        else
        {

        }
	}

    public void EffectsPlay()
    {
        effectsPlaying = true;
        effectsPlay.text = "Next";
        if (effectsPlaying)
        {
            loopsPlaying = false;
            loopsPlay.text = "Play";
        }
        timestamp = -1000;
        this.GetComponent<AudioSource>().Stop();
        effectsCurrent.text = "";
        loopsCurrent.text = "";
    }

    public void LoopsPlay()
    {
        loopsPlaying = true;
        loopsPlay.text = "Next";
        if (effectsPlaying)
        {
            effectsPlaying = false;
            effectsPlay.text = "Play";
        }
        timestamp = -1000;
        this.GetComponent<AudioSource>().Stop();
        effectsCurrent.text = "";
        loopsCurrent.text = "";
    }
}
