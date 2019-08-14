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
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "Audio Source component not found!");
        audioSource.Stop();
    }
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
    
    public void PlayAudio(string name)
    {
        /*
        bool clipFound = false;
        for(int i = 0; i < audioClips.Length; i++)
        {
            if(audioClips[i].name == name)
            {
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
        */
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
