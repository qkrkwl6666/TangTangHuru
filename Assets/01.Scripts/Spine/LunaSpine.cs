using Spine;
using Spine.Unity;
using UnityEngine;

public class LunaSpine : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, run;
    public PlayerController controller;
    public PlayerState previousState = PlayerState.Idle;
    private bool isAttacking = false;

    public string CurrentCharacterSkin { get; private set; } = string.Empty;

    public string CurrentWeaponSkin { get; private set; } = string.Empty;

    private void Awake()
    {

    }

    private void Start()
    {
        SetCharacterWeaponSkin("body_036", "weapon_015");

        Defines.SetSkins();
    }

    private void Update()
    {
        if (skeletonAnimation == null || controller == null) return;

        PlayerState currentState = controller.state;

        if (currentState != previousState)
        {
            PlayAnimation(currentState);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            int characterIndex = Random.Range(0, Defines.characterSkins.Count);
            int weaponIndex = Random.Range(0, Defines.weaponSkins.Count);

            var characterSkin = Defines.characterSkins[characterIndex];
            var weaponSkin = Defines.weaponSkins[weaponIndex];

            SetCharacterWeaponSkin(characterSkin, weaponSkin);

        }
    }

    public void PlayAnimation(PlayerState currentState)
    {
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

    public void SetCharacterWeaponSkin(string chSkin, string wepSkin)
    {
        CurrentCharacterSkin = chSkin;
        CurrentWeaponSkin = wepSkin;

        Spine.Skin characterSkin = skeletonAnimation.skeleton.Data.FindSkin(CurrentCharacterSkin);
        Spine.Skin weaponSkin = skeletonAnimation.skeleton.Data.FindSkin(CurrentWeaponSkin);

        Spine.Skin combinedSkin = new Spine.Skin("character_with_weapon");

        combinedSkin.AddSkin(weaponSkin);
        combinedSkin.AddSkin(characterSkin);

        // 결합된 스킨 적용
        skeletonAnimation.Skeleton.SetSkin(combinedSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    public void SetCharacterSkin(string chSkin)
    {
        if (CurrentCharacterSkin == chSkin) return;

        SetCharacterWeaponSkin(chSkin, CurrentWeaponSkin);
    }

    public void SetWeaponSkin(string wepSkin)
    {
        if (CurrentWeaponSkin == wepSkin) return;

        SetCharacterWeaponSkin(CurrentCharacterSkin, wepSkin);
    }

}