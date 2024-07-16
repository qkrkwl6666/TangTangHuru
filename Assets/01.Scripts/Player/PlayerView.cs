using Spine.Unity;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, run, hit, attack;
    public PlayerController controller;
    public PlayerState previousState = PlayerState.Idle;
    private bool isAttacking = false;

    private void Awake()
    {

    }

    private void Start()
    {
        Spine.Skin characterSkin = skeletonAnimation.skeleton.Data.FindSkin("body_001");
        Spine.Skin weaponSkin = skeletonAnimation.skeleton.Data.FindSkin("weapon_005");

        Spine.Skin combinedSkin = new Spine.Skin("character_with_weapon");

        combinedSkin.AddSkin(weaponSkin);
        combinedSkin.AddSkin(characterSkin);

        // 결합된 스킨 적용
        skeletonAnimation.Skeleton.SetSkin(combinedSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    private void Update()
    {
        if (skeletonAnimation == null || controller == null) return;

        PlayerState currentState = controller.state;

        if (currentState != previousState /* || Input.GetKeyDown(KeyCode.Space)*/) 
        {
            PlayAnimation(currentState);
        }
    }

    public void PlayAnimation(PlayerState currentState)
    {
        previousState = currentState;

        switch (currentState)
        {
            case PlayerState.Idle:
                skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
                //if (Input.GetKeyDown(KeyCode.Space)) // 달리는 중 스페이스바를 누르면 공격
                //{
                //    PlayAttackAnimation();
                //}
                break;
            case PlayerState.Run:
                skeletonAnimation.AnimationState.SetAnimation(0, run, true);
                //if (Input.GetKeyDown(KeyCode.Space)) // 달리는 중 스페이스바를 누르면 공격
                //{
                //    PlayAttackAnimation();
                //}
                break;
        }
    }

    private void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            var attackTrack = skeletonAnimation.AnimationState.SetAnimation(1, attack, false);
            attackTrack.Complete += OnAttackComplete; // 공격 애니메이션이 끝나면 호출될 이벤트

            // 공격 애니메이션과 달리기 애니메이션을 부드럽게 블렌딩
            attackTrack.MixDuration = 0.1f;
        }
    }

    private void OnAttackComplete(Spine.TrackEntry trackEntry)
    {
        isAttacking = false;
        skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0.1f); // 공격 애니메이션 트랙을 비웁니다
    }
}