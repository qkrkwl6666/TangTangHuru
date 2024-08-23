using Spine.Unity;
using UnityEngine;

public class BossView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private float time = 0f;

    public Spine.TrackEntry PlayAnimation(string name, bool loop = false)
    {
        isDash = false;

        return skeletonAnimation.state.SetAnimation(0, name, loop);
    }


    #region ���� ��ȣ�� �ִϸ��̼�
    private float startTime = 0.815f; // �ùķ��̼� 50�� �������鼭 ������ ���� ã�ҽ��ϴ�.
    private float endTime = 1.265f;

    private bool isDash = false;

    private void Start()
    {

    }

    public void PlayDashAnimation()
    {
        var track = skeletonAnimation.AnimationState.SetAnimation(0, Defines.attack1, true);

        isDash = true;
    }

    public void StopDashAnimation()
    {
        skeletonAnimation.AnimationState.ClearTracks();

        skeletonAnimation.Skeleton.SetToSetupPose();

        isDash = false;
    }

    void Update()
    {
        if (!isDash) return;

        var currentTrack = skeletonAnimation.AnimationState.GetCurrent(0);

        if (currentTrack != null && currentTrack.Animation.Name == Defines.attack1)
        {
            if (currentTrack.TrackTime >= endTime)
            {
                currentTrack.TrackTime = 0.81f;
            }
        }

    }

    #endregion
}
