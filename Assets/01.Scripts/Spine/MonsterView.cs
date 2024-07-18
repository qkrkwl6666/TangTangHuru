using Spine.Unity;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    private void Awake()
    {

    }

    public void Update()
    {
        //if(Input.GetKeyDown(KeyCode.F2))
        //{
        //    PlayAnimation(Defines.idle, true);
        //}
        //
        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    PlayAnimation(Defines.attack, false);
        //}
        //
        //if (Input.GetKeyDown(KeyCode.F4))
        //{
        //    PlayAnimation(Defines.walk, true);
        //}
    }

    public Spine.TrackEntry PlayAnimation(string name, bool loop = false)
    {
        return skeletonAnimation.state.SetAnimation(0, name, loop);
    }
}
