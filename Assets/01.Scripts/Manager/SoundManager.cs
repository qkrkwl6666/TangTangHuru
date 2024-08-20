using System.Collections;
using System.Collections.Generic;
using System.Media;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum SoundType
{
    BGM,
    EFFECT,
}

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer mAudioMixer;

    private float currBGMVolume, currEffectVolume;

    private Dictionary<string, AudioClip> clipDictionary;

    [SerializeField] private List<AudioClip> preloadClips = new();

    private List<TemporalSoundPlayer> currPlayingSounds;
    private Queue<TemporalSoundPlayer> soundPlayerPool;

    [SerializeField] private int poolSize = 10;

    private int loadCount = 0;

    private void Start()
    {
        clipDictionary = new Dictionary<string, AudioClip>();
        currPlayingSounds = new List<TemporalSoundPlayer>();
        soundPlayerPool = new Queue<TemporalSoundPlayer>();

        // �̸� ���� �÷��̾� ������Ʈ ����
        Addressables.LoadAssetAsync<GameObject>("TemporalSoundPlayer").Completed += (x) =>
        {
            for (int i = 0; i < poolSize; ++i)
            {
                var tempPlayer = Instantiate(x.Result);
                tempPlayer.SetActive(false);
                var tempSoundPlayer = tempPlayer.GetComponent<TemporalSoundPlayer>();
                soundPlayerPool.Enqueue(tempSoundPlayer);
            }
        };

        Addressables.LoadAssetsAsync<AudioClip>("LobbySound", OnClipLoaded).Completed += OnLoadCheck;
        Addressables.LoadAssetsAsync<AudioClip>("BGM", OnClipLoaded).Completed += OnLoadCheck;

        //var handle = Addressables.LoadAssetsAsync<AudioClip>("BGM", OnClipLoaded);
        //handle.Completed += OnClipsLoaded;
        //handle.Completed += (operation) => PlaySound2D("BGM_main", 0, true, SoundType.BGM);

    }

    private void OnLoadCheck(AsyncOperationHandle<IList<AudioClip>> handle)
    {
        loadCount++;

        if (loadCount == 2)
        {
            OnClipsLoaded(handle);
        }
    }

    private void OnClipLoaded(AudioClip clip)
    {
        preloadClips.Add(clip);
    }

    private void OnClipsLoaded(AsyncOperationHandle<IList<AudioClip>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (AudioClip clip in preloadClips)
            {
                clipDictionary.Add(clip.name, clip);
            }
            Debug.Log("All sounds loaded successfully.");

            if(currPlayingSounds.Count == 0)
            {
                PlaySound2D("BGM_main", 0, true, SoundType.BGM);
            }
        }
        else
        {
            Debug.LogError("Failed to load sounds.");
        }
    }



    private TemporalSoundPlayer GetSoundPlayer()
    {
        if (soundPlayerPool.Count > 0)
        {
            var soundPlayer = soundPlayerPool.Dequeue();
            soundPlayer.gameObject.SetActive(true);
            return soundPlayer;
        }
        else
        {
            // Ǯ�� ���� ������Ʈ�� ������ ���� ����
            GameObject obj = new GameObject("TemporalSoundPlayer 2D");
            return obj.AddComponent<TemporalSoundPlayer>();
        }
    }

    private void ReturnSoundPlayer(TemporalSoundPlayer soundPlayer)
    {
        soundPlayer.gameObject.SetActive(false);
        soundPlayerPool.Enqueue(soundPlayer);
    }

    private AudioClip GetClip(string clipName)
    {
        if (clipDictionary.TryGetValue(clipName, out var clip))
        {
            return clip;
        }

        Debug.LogError(clipName + "�� �������� �ʽ��ϴ�.");
        return null;
    }

    private void AddToList(TemporalSoundPlayer soundPlayer)
    {
        currPlayingSounds.Add(soundPlayer);
    }

    public void StopLoopSound(string clipName)
    {
        for (int i = 0; i < currPlayingSounds.Count; i++)
        {
            if (currPlayingSounds[i].ClipName == clipName)
            {
                var soundPlayer = currPlayingSounds[i];
                currPlayingSounds.RemoveAt(i);
                ReturnSoundPlayer(soundPlayer);
                return;
            }
        }

        Debug.LogWarning(clipName + "�� ã�� �� �����ϴ�.");
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        var soundPlayer = GetSoundPlayer();

        // ������ ����ϴ� ��� ���带 ����
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);

        if (!isLoop)
        {
            // ������ �ƴ� ��� ���� �ð� �� ���� �÷��̾� ��ȯ
            StartCoroutine(ReturnSoundPlayerAfterPlay(soundPlayer));
        }
    }

    private IEnumerator ReturnSoundPlayerAfterPlay(TemporalSoundPlayer soundPlayer)
    {
        yield return new WaitUntil(() => !soundPlayer.AudioSource.isPlaying);
        ReturnSoundPlayer(soundPlayer);
    }

    public void InitVolumes(float bgmVolume, float effectVolume)
    {
        SetVolume(SoundType.BGM, bgmVolume);
        SetVolume(SoundType.EFFECT, effectVolume);
    }

    public void SetVolume(SoundType type, float value)
    {
        mAudioMixer.SetFloat(type.ToString(), value);
    }
}
