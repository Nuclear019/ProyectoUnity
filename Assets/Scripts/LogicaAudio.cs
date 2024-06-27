using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicaAudio : MonoBehaviour
{

    [SerializeField] private Slider musicSlider;
    [SerializeField] private  Slider audioSlider;

    private float musicVol;
    private float audioVol;

    void Update()
    {
        musicVol = musicSlider.value;
        audioVol = audioSlider.value;
    }

    public float GetMusicVol()
    {
        return musicVol;
    }
    public float GetAudioVol()
    {
        return audioVol;
    }
}
