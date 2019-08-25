using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BgMusic : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        audioSource.volume = 0.3f;
    }
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
    
    public void PlayAudio(string name)
    {
        bool clipFound = false;
        for(int i = 0; i < audioClips.Length; i++)
        {
            if(audioClips[i].name == name)
            {
                //audioSource.Stop();
                Debug.Log("Audio " + name + " started");
                //maybe needs check if already playing that
                audioSource.clip = audioClips[i];
                audioSource.Play();
                clipFound = true;
            }
        }
        if (!clipFound)
        {
            Debug.Log("Audio " + name + " not found");
        }
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PauseAudio()
    {
        audioSource.Pause();
    }

    public void ResumeAudio()
    {
        audioSource.Play();
    }

}
