using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeConfiguration : MonoBehaviour
{
    public AudioMixer mixer;
    public string exposedParam;
    [Space]
    public Slider slider;

    private void Start()
    {
        mixer.GetFloat(exposedParam, out float value);
        slider.value = value;
    }

    public void ChangeVolume(float value)
    {
        mixer.SetFloat(exposedParam, value);
    }

}
