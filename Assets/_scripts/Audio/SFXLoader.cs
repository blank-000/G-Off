using System.Collections;
using UnityEngine;

public class SFXLoader : MonoBehaviour
{
    public AudioClip spawningClip;
    public AudioClip normalClip;

    AudioSource source;


    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = spawningClip;
        source.Play();

        StartCoroutine(SwitchClipWhenFinished());
    }

    IEnumerator SwitchClipWhenFinished()
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        source.clip = normalClip;
    }


}
