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
        source.pitch += Random.Range(-0.1f, 0.1f);


        StartCoroutine(PlayClipWithRandomDelay());


    }
    IEnumerator PlayClipWithRandomDelay()
    {
        yield return new WaitForSecondsRealtime(Random.Range(0, .4f));
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
