using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TemporalSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioSource AudioSource {  get { return audioSource; } }
    public string ClipName
    {
        get
        {
            return audioSource.clip.name;
        }
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.loop = isLoop;
        audioSource.Play();

        if (!isLoop) { StartCoroutine(COR_DestroyWhenFinish(audioSource.clip.length)); }
    }

    public void InitSound2D(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void InitSound3D(AudioClip clip, float minDistance, float maxDistance)
    {
        audioSource.clip = clip;
        audioSource.spatialBlend = 1.0f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
    }

    private IEnumerator COR_DestroyWhenFinish(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }
}
