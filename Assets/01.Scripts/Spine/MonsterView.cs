using Spine.Unity;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private float timeInterval = 1 / 30f;
    private float deltaTime = 0f;

    private void Awake()
    {
        // skeletonAnimation.Initialize(false);
        // skeletonAnimation.clearStateOnDisable = false;
        // skeletonAnimation.enabled = false;
    }

    public void Update()
    {

        //deltaTime += Time.deltaTime;
        //if (deltaTime >= timeInterval)
        //{
        //    ManualUpdate();
        //}
    }

    public void ManualUpdate()
    {
        skeletonAnimation.Update(deltaTime);
        skeletonAnimation.LateUpdate();
        deltaTime = 0f;
    }

    public Spine.TrackEntry PlayAnimation(string name, bool loop = false)
    {
        return skeletonAnimation.state.SetAnimation(0, name, loop);
    }
}
