using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip roundStart;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RoundStart()
    {
        if (audioSource) 
        {
            audioSource.PlayOneShot(roundStart);
        }
    }
}
