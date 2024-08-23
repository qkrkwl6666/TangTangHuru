using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider SFXSlider;

    private float BGMValue = 0;
    private float SFXValue = 0;

    void Update()
    {
        BGMValue = Mathf.Lerp(-80, 0, BGMSlider.value);
        SFXValue = Mathf.Lerp(-80, 0, SFXSlider.value);
        SoundManager.Instance.SetVolume(SoundType.BGM, BGMValue);
        SoundManager.Instance.SetVolume(SoundType.EFFECT, SFXValue);

        SoundManager.Instance.currEffectVolume = SFXSlider.value;
    }
}
