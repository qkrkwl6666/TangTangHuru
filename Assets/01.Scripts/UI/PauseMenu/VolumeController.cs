using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;

    void Update()
    {
        SoundManager.Instance.SetVolume(SoundType.BGM, (BGMSlider.value * 10));
        SoundManager.Instance.SetVolume(SoundType.EFFECT, SFXSlider.value);
    }
}
