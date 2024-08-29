using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    public Spine.AnimationState animationState;
    public List<SkeletonRenderer> skeletonRenderer;

    private int frameDuration = 5;
    private int currentFrame = 0;
    private float deltaTime = 0f;

    private void Awake()
    {
        // animationState가 아직 할당되지 않았다면 할당
        if (animationState == null)
        {
            animationState = new Spine.AnimationState(skeletonRenderer[0].skeletonDataAsset.GetAnimationStateData());
        }
    }

    private void Start()
    {
        foreach (var skeletonRenderer in skeletonRenderer)
        {
            // 스켈레톤에 AnimationState 적용
            animationState.Apply(skeletonRenderer.skeleton);
            skeletonRenderer.skeleton.UpdateWorldTransform();
        }

        // 애니메이션 설정
        SetupAnimation();
    }

    private void SetupAnimation()
    {
        foreach (var renderer in skeletonRenderer)
        {
            renderer.skeleton.SetToSetupPose();
        }

        // 스켈레톤 데이터에서 애니메이션 가져오기
        Spine.Animation animation = skeletonRenderer[0].skeleton.Data.FindAnimation(Defines.idle);

        if (animation != null)
        {
            animationState.SetAnimation(0, animation, true);
        }
        else
        {
            //Debug.LogWarning("Animation not found!");
        }
    }

    private void Update()
    {
        currentFrame++;
        deltaTime += Time.deltaTime;

        if (currentFrame >= frameDuration)
        {
            animationState.Update(deltaTime);

            foreach (var skeletonRenderer in skeletonRenderer)
            {
                animationState.Apply(skeletonRenderer.skeleton);
                skeletonRenderer.skeleton.UpdateWorldTransform();
            }

            currentFrame = 0;
            deltaTime = 0f;
        }
    }

    private void FixedUpdate()
    {

    }
}
