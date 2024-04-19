using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Scrollbar musicSlider;

    public void musicSetting()
    {
        float volume = musicSlider.value;
        if(volume == 0)
        {
            volume = 0.01f;
        }
        audioMixer.SetFloat("music", Mathf.Log10(volume)*22+8);
    }
}
