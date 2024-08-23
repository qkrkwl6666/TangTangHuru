using Spine.Unity;
using UnityEngine;

public class PlayerViewUI : MonoBehaviour
{
    public SkeletonGraphic skeletonAnimation;
    public AnimationReferenceAsset idle, run;
    public PlayerController controller;
    public PlayerState previousState = PlayerState.Idle;
    public string CurrentCharacterSkin { get; private set; } = string.Empty;
    public string CurrentWeaponSkin { get; private set; } = string.Empty;

    private void Awake()
    {

    }

    private void Start()
    {
        SetNoneWeaponCharacterSkin(GameManager.Instance.characterSkin);

        //SetCharacterWeaponSkin(Defines.body033, Defines.weapon005);

        //Defines.SetSkins();
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

        Spine.Skin characterSkin = skeletonAnimation.Skeleton.Data.FindSkin(CurrentCharacterSkin);

        if (CurrentWeaponSkin == string.Empty)
        {
            skeletonAnimation.Skeleton.SetSkin(characterSkin);
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            return;
        }

        Spine.Skin weaponSkin = skeletonAnimation.Skeleton.Data.FindSkin(CurrentWeaponSkin);

        Spine.Skin combinedSkin = new Spine.Skin("character_with_weapon");

        combinedSkin.AddSkin(weaponSkin);
        combinedSkin.AddSkin(characterSkin);

        // 결합된 스킨 적용
        skeletonAnimation.Skeleton.SetSkin(combinedSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    public void SetCharacterSkin(string chSkin)
    {
        SetCharacterWeaponSkin(chSkin, CurrentWeaponSkin);
    }

    public void SetNoneWeaponCharacterSkin(string chSkin)
    {
        CurrentCharacterSkin = chSkin;
        CurrentWeaponSkin = string.Empty;

        Spine.Skin characterSkin = skeletonAnimation.Skeleton.Data.FindSkin(CurrentCharacterSkin);

        skeletonAnimation.Skeleton.SetSkin(characterSkin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    public void SetWeaponSkin(string wepSkin)
    {
        if (CurrentWeaponSkin == wepSkin) return;

        SetCharacterWeaponSkin(CurrentCharacterSkin, wepSkin);
    }

    public void RandomCharacter()
    {
        int characterIndex = Random.Range(0, Defines.characterSkins.Count);
        int weaponIndex = Random.Range(0, Defines.weaponSkins.Count);

        var characterSkin = Defines.characterSkins[characterIndex];
        var weaponSkin = Defines.weaponSkins[weaponIndex];

        GameManager.Instance.characterSkin = characterSkin;
        GameManager.Instance.weaponSkin = weaponSkin;

        SetCharacterWeaponSkin(characterSkin, weaponSkin);
    }

}