using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MonsterSkeletonSharing : MonoBehaviour
{
    private Dictionary<string, SkeletonAnimation> monsterSkeletonAnimation = new ();
    public Dictionary<string, SkeletonDataAsset> monsterSkeletonDataAsset = new ();
    private Dictionary<string, TaskCompletionSource<SkeletonAnimation>> loadingTasks = new();
    public List<SkeletonRenderer> skeletonRenderers = new List<SkeletonRenderer>();

    private Spine.AnimationState sharedAnimationState;

    private void Awake()
    {
        
    }

    private void Update()
    {
        foreach (var skeletonAnimation in monsterSkeletonAnimation) 
        {
            skeletonAnimation.Value.Update(Time.deltaTime);
        }

        foreach(var skeletonRender in skeletonRenderers)
        {
            sharedAnimationState.Apply(skeletonRender.skeleton);

            skeletonRender.skeleton.UpdateWorldTransform();
        }


    }

    public async Task<SkeletonAnimation> GetSkeletonAnimationAsync(string key)
    {
        // 이미 있으면 return
        if (monsterSkeletonAnimation.TryGetValue(key, out SkeletonAnimation animation)) 
        {
            return animation;
        }

        // 현재 로딩 중인 작업 체크
        if (loadingTasks.TryGetValue(key, out var existingTask))
        {
            return await existingTask.Task;
        }

        var tcs = new TaskCompletionSource<SkeletonAnimation>();
        loadingTasks[key] = tcs;

        try
        {
            var skeletonDataAsset = await Addressables.LoadAssetAsync<SkeletonDataAsset>($"{key}{Defines.skeletonData}").Task;
            var sharing = new GameObject($"Shared Skeleton - {key}").AddComponent<SkeletonAnimation>();
            sharing.skeletonDataAsset = skeletonDataAsset;
            monsterSkeletonDataAsset.Add(key, skeletonDataAsset);
            sharing.Initialize(false);

            monsterSkeletonAnimation[key] = sharing;
            tcs.SetResult(sharing);
            loadingTasks.Remove(key);

            sharedAnimationState = new Spine.AnimationState(skeletonDataAsset.GetAnimationStateData());

            sharedAnimationState.SetAnimation(0, Defines.walk, true);

            return sharing;
        }
        catch(Exception ex)
        {
            tcs.SetException(ex);
            loadingTasks.Remove(key);
            throw;
        }
    }


}
