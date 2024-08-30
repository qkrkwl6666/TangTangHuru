using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkeletonSharing : MonoBehaviour
{
    //private Dictionary<string, SkeletonAnimation> monsterSkeletonAnimation = new ();
    //private Dictionary<string, TaskCompletionSource<SkeletonAnimation>> loadingTasks = new();

    private Dictionary<string, List<SkeletonRenderer>> skeletonRenderers = new();
    private Dictionary<string, Spine.AnimationState> sharedAnimationState = new();

    private int frameDuration = 5;
    private int currentFrame = 0;
    private float deltaTime = 0f;

    private void Awake()
    {

    }

    private void Update()
    {
        currentFrame++;
        deltaTime += Time.deltaTime;

        if (currentFrame >= frameDuration)
        {
            foreach (var animationState in sharedAnimationState)
            {
                animationState.Value.Update(deltaTime);
            }

            foreach (var skeletonRendererList in skeletonRenderers)
            {
                string key = skeletonRendererList.Key;

                foreach (var skeletonRenderer in skeletonRendererList.Value)
                {
                    //if(!skeletonRenderer.gameObject.activeSelf) continue;

                    sharedAnimationState[key].Apply(skeletonRenderer.skeleton);
                    skeletonRenderer.skeleton.UpdateWorldTransform();
                }
            }

            currentFrame = 0;
            deltaTime = 0f;
        }

    }

    // ���̷��� �������� ������ Spine.AnimationState �����ϰ� �ִϸ��̼� ���� �ϱ�
    public void AddSkeletonRenderers(string key, SkeletonRenderer skeletonRenderer)
    {
        if (skeletonRenderers.ContainsKey(key))
        {
            skeletonRenderers[key].Add(skeletonRenderer);
            return;
        }

        // ������ ����
        //skeletonRenderers.ad

        skeletonRenderers[key] = new List<SkeletonRenderer>();
        skeletonRenderers[key].Add(skeletonRenderer);

        sharedAnimationState.Add(key, new Spine.AnimationState(skeletonRenderer
            .skeletonDataAsset.GetAnimationStateData()));

        //Debug.Log(skeletonRenderer.skeleton);

        sharedAnimationState[key].Apply(skeletonRenderer.skeleton);
        skeletonRenderer.skeleton.UpdateWorldTransform();

        // �ִϸ��̼� ����
        skeletonRenderer.skeleton.SetToSetupPose();

        // ���̷��� �����Ϳ��� �ִϸ��̼� ��������

        //string name = key == "119" ? Defines.walk2 : Defines.walk;

        Spine.Animation animation = skeletonRenderer.skeleton.Data.
            FindAnimation(Defines.walk);

        sharedAnimationState[key].SetAnimation(0, animation, true);
    }

}
