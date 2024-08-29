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
        // animationState�� ���� �Ҵ���� �ʾҴٸ� �Ҵ�
        if (animationState == null)
        {
            animationState = new Spine.AnimationState(skeletonRenderer[0].skeletonDataAsset.GetAnimationStateData());
        }
    }

    private void Start()
    {
        foreach (var skeletonRenderer in skeletonRenderer)
        {
            // ���̷��濡 AnimationState ����
            animationState.Apply(skeletonRenderer.skeleton);
            skeletonRenderer.skeleton.UpdateWorldTransform();
        }

        // �ִϸ��̼� ����
        SetupAnimation();
    }

    private void SetupAnimation()
    {
        foreach (var renderer in skeletonRenderer)
        {
            renderer.skeleton.SetToSetupPose();
        }

        // ���̷��� �����Ϳ��� �ִϸ��̼� ��������
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
