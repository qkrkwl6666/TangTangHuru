using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, run, hit, attack;
    public PlayerController controller;
    public PlayerState previousState = PlayerState.Idle;

    private void Awake()
    {
        //PlayAnimation(idle, true);
    }

    private void Update()
    {
        if (skeletonAnimation == null || controller == null) return;

        PlayerState currentState = controller.state;

        if (currentState != previousState)
        {
            PlayAnimation();
        }
    }

    public void PlayAnimation()
    {
        PlayerState currentState = controller.state;
        previousState = currentState;
        switch (currentState)
        {
            case PlayerState.Idle:
                skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
                
                break;
            case PlayerState.Run:
                skeletonAnimation.AnimationState.SetAnimation(0, run, true);
                break;
        }
    }

}
