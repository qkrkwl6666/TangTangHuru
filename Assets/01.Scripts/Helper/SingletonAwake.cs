using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

public class SingletonAwake : MonoBehaviour
{
    private void Awake()
    {
        var gm = GameManager.Instance;

        var saveMgr = SaveManager.Instance;

        var dtm = DataTableManager.Instance;

        Application.targetFrameRate = 60;

        DataTableManager.Instance.OnAllTableLoaded += () =>
        {
            var achieveMgr = AchievementManager.Instance;
        };

        var sound = SoundManager.Instance;

        Addressables.LoadAssetAsync<AudioMixer>("AudioMixer").Completed += (x) =>
        {
            sound.mAudioMixer = x.Result;
        };
    }


}
