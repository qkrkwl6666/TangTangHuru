using Spine.Unity;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private float timeInterval = 1 / 30f;
    private float deltaTime = 0f;

    private void Awake()
    {

    }

    public void Update()
    {

    }

    public Spine.TrackEntry PlayAnimation(string name, bool loop = false)
    {
        return skeletonAnimation.state.SetAnimation(0, name, loop);
    }
}
