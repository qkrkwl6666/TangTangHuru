using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject tempPlayer;

    private int loadCount = 0;

    private bool soundLoaded = false;

    private void Start()
    {
        clipDictionary = new Dictionary<string, AudioClip>();
        currPlayingSounds = new List<TemporalSoundPlayer>();
        soundPlayerPool = new Queue<TemporalSoundPlayer>();

        // 미리 사운드 플레이어 오브젝트 생성
        CreateTemporalObjects();

        Addressables.LoadAssetsAsync<AudioClip>("LobbySound", OnClipLoaded).Completed += OnLoadCheck;
        Addressables.LoadAssetsAsync<AudioClip>("BGM", OnClipLoaded).Completed += OnLoadCheck;
        Addressables.LoadAssetsAsync<AudioClip>("StageSound", OnClipLoaded).Completed += OnLoadCheck;
        Addressables.LoadAssetsAsync<AudioClip>("WeaponSound", OnClipLoaded).Completed += OnLoadCheck;
    }

    public void CreateTemporalObjects()
    {
        if(tempPlayer != null)
        {
            for (int i = 0; i < poolSize; ++i)
            {
                var temp = Instantiate(tempPlayer);
                temp.SetActive(false);
                var tempSoundPlayer = tempPlayer.GetComponent<TemporalSoundPlayer>();
                soundPlayerPool.Enqueue(tempSoundPlayer);
            }
            return;
        }

        Addressables.LoadAssetAsync<GameObject>("TemporalSoundPlayer").Completed += (x) =>
        {
            for (int i = 0; i < poolSize; ++i)
            {
                tempPlayer = Instantiate(x.Result);
                tempPlayer.SetActive(false);
                var tempSoundPlayer = tempPlayer.GetComponent<TemporalSoundPlayer>();
                soundPlayerPool.Enqueue(tempSoundPlayer);
            }
        };
    }

    public void ClearSoundPlayerPool()
    {
        soundPlayerPool.Clear();
    }

    private void OnLoadCheck(AsyncOperationHandle<IList<AudioClip>> handle)
    {
        loadCount++;

        if (loadCount == 4)
        {
            OnClipsLoaded(handle);
        }
    }
    public void EnterStage()
    {
        if (soundLoaded)
            return;

        Addressables.LoadAssetsAsync<AudioClip>("StageSound", OnClipLoaded).Completed += OnLoadCheck;
        Addressables.LoadAssetsAsync<AudioClip>("WeaponSound", OnClipLoaded).Completed += OnLoadCheck;

        soundLoaded = true;
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
            // 풀에 남은 오브젝트가 없으면 새로 생성
            var temp = Instantiate(tempPlayer);
            return temp.GetComponent<TemporalSoundPlayer>();
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

        Debug.LogError(clipName + "이 존재하지 않습니다.");
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

        Debug.LogWarning(clipName + "을 찾을 수 없습니다.");
    }

    public void PlayerBGM(int index)
    {
        currPlayingSounds.Clear();

        string BGM_Name = "BGM_Stage_";

        switch (index)
        {
            case 0:
                BGM_Name = "BGM_main";
                break;
            default:
                BGM_Name += index.ToString();
                break;
        }

        PlaySound2D(BGM_Name, 0, true, SoundType.BGM);
    }

    public void PlaySound2D(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.EFFECT)
    {
        var soundPlayer = GetSoundPlayer();

        // 루프를 사용하는 경우 사운드를 저장
        if (isLoop) { AddToList(soundPlayer); }

        soundPlayer.InitSound2D(GetClip(clipName));
        soundPlayer.Play(mAudioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);

        if (!isLoop)
        {
            // 루프가 아닌 경우 일정 시간 후 사운드 플레이어 반환
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
