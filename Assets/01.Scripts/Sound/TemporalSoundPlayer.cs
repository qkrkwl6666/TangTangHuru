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
    }

    public void InitSound2D(AudioClip clip)
    {
        audioSource.clip = clip;
    }
}
