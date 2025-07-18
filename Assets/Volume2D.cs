using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume2D : MonoBehaviour
{
    public Transform listenerTransform;
    public AudioSource audioSource;
    public float minDist=1;
    public float maxDist=20;
    public float OGVolume;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        OGVolume = audioSource.volume;
        listenerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, listenerTransform.position);

        if(dist < minDist)
        {
            audioSource.volume = OGVolume;
        }
        else if(dist > maxDist)
        {
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = OGVolume - (((dist - minDist) / (maxDist - minDist)) / OGVolume);
        }
    }
}