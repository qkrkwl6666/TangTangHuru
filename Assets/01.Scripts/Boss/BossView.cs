using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public Spine.TrackEntry PlayAnimation(string name, bool loop = false)
    {
        return skeletonAnimation.state.SetAnimation(0, name, loop);
    }
}
