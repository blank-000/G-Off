using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer Mixer;
    public string TargetParameter;
    public int minVolumeDb, maxVolumeDb;
    Slider volumeSlider;

    void Awake()
    {
        volumeSlider = GetComponent<Slider>();

    }
    void Start()
    {
        SetVolume(volumeSlider.value);
    }

    public void SetVolume(float sliderValue)
    {
        float dbValue = Mathf.Lerp(minVolumeDb, maxVolumeDb, sliderValue);
        Mixer.SetFloat(TargetParameter, dbValue);
    }

    void OnDestroy()
    {
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}
