using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider sfxSlider;
    public Slider masterSlider;
    public Slider musicSlider;


    void Start()
    {
        sfxSlider.value = 1;
        masterSlider.value = 1;
        musicSlider.value = 1;

        SetSFX(sfxSlider.value);
        SetMaster(masterSlider.value);
        SetMusic(musicSlider.value);

    }


    public void SetSFX(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }


    public void SetMaster(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }


    public void SetMusic(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}