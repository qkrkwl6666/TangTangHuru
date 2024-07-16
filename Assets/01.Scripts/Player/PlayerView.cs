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

        // ���յ� ��Ų ����
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
                //if (Input.GetKeyDown(KeyCode.Space)) // �޸��� �� �����̽��ٸ� ������ ����
                //{
                //    PlayAttackAnimation();
                //}
                break;
            case PlayerState.Run:
                skeletonAnimation.AnimationState.SetAnimation(0, run, true);
                //if (Input.GetKeyDown(KeyCode.Space)) // �޸��� �� �����̽��ٸ� ������ ����
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
            attackTrack.Complete += OnAttackComplete; // ���� �ִϸ��̼��� ������ ȣ��� �̺�Ʈ

            // ���� �ִϸ��̼ǰ� �޸��� �ִϸ��̼��� �ε巴�� ����
            attackTrack.MixDuration = 0.1f;
        }
    }

    private void OnAttackComplete(Spine.TrackEntry trackEntry)
    {
        isAttacking = false;
        skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0.1f); // ���� �ִϸ��̼� Ʈ���� ���ϴ�
    }
}