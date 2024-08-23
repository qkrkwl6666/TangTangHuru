using Spine.Unity;
using UnityEngine;

public class GuardianView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public void PlayAnimation(string name, bool loop)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, name, loop);
    }
}
