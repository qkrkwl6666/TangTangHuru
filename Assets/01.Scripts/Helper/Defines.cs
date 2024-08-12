using System.Collections.Generic;
using UnityEngine;

public static class Defines
{
    public static readonly string exp = "Exp";
    public static readonly string normalBullet = "NormalBullet";
    public static readonly string snailBullet = "SnailBullet";
    public static readonly string treasure = "Treasure";
    public static readonly string well = "Well";
    public static readonly string bossWall = "BossWall";
    public static readonly string playBoss = "PlayBoss";
    public static readonly string obstacles = "Obstacles";
    public static readonly string rangeArea = "RangeArea";

    public static readonly string loadingUI = "LoadingUI";
    public static readonly string main = "Main";
    public static readonly string inGame = "InGame";
    public static readonly string joystick = "Joystick";
    public static readonly string skeletonData = "SkeletonData";
    public static readonly string stageImage = "StageImage";
    public static readonly string emptyRect = "EmptyRect";

    // 아웃라이너 컬러
    public static readonly Color blueColor = new Color(2 / 255f, 19 / 255f, 52 / 255f);
    public static readonly Color greenColor = new Color(20 / 255f, 46 / 255f, 34 / 255f);
    public static readonly Color orangeColor = new Color(45 / 255f, 30 / 255f, 18 / 255f);
    public static readonly Color purpleColor = new Color(33 / 255f, 13 / 255f, 52 / 255f);
    public static readonly Color redColor = new Color(39 / 255f, 10 / 255f, 8 / 255f);
    public static readonly Color whiteColor = Color.white;
    public static readonly Color yellowColor = new Color(40 / 255f, 36 / 255f, 29 / 255f);
    public static readonly Color allFoucsTextColor = new Color(246 / 255f, 225 / 255f, 156 / 255f);
    public static readonly Color allNormalTextColor = new Color(155 / 255f, 155 / 255f, 155 / 255f);

    // 무기 스텟 텍스트
    public static readonly string damage = "공격력 : ";
    public static readonly string attackCoolTime = "공격 속도 : ";
    public static readonly string criticalChance = "치명타 확률 : ";
    public static readonly string criticalDamage = "치명타 피해 : ";

    // 방어구 스텟 텍스트
    public static readonly string hp = "체력 : ";
    public static readonly string defence = "방어력 : ";
    public static readonly string dodge = "회피율 : ";

    public static int MaxUpgrade = 10;
    public static int MaxWeaponCount = 6;

    // 업그레이드 비용
    public static int defaultUpgradeGold = 5000;
    public static int defaultUpgradeReinforcedStone = 1;

    // 방어구 스텟 텍스트

    public static readonly string itemSlot = "ItemSlot";
    public static readonly string gemStoneSlot = "GemStoneSlot";

    // 장비 슬롯 텍스처
    public static readonly string equipSlotArmor = "EquipSlotArmor";
    public static readonly string equipSlotBg = "EquipSlotBg";
    public static readonly string equipSlotBoots = "EquipSlotBoots";
    public static readonly string equipSlotBorder = "EquipSlotBorder";
    public static readonly string equipSlotHelmet = "EquipSlotHelmet";
    public static readonly string equipSlotInnerBorder = "EquipSlotInnerBorder";
    public static readonly string equipSlotSword = "EquipSlotInnerBorder";


    // 오브 개수 제한
    public static int normalOrbCount = 0;
    public static int rareOrbCount = 1;
    public static int epicOrbCount = 2;
    public static int uniqueOrbCount = 3;
    public static int legendaryOrbCount = 4;

    // 마법사 캐릭터 스킨
    public static readonly string body001 = "body_001";
    public static readonly string body024 = "body_024";
    public static readonly string body033 = "body_033";
    public static readonly string body036 = "body_036";
    public static readonly string body038 = "body_038";
    public static readonly string body039 = "body_039";
    public static readonly string body040 = "body_040";
    public static readonly string body043 = "body_043";

    // 마법사 무기 
    public static readonly string weapon005 = "weapon_005";
    public static readonly string weapon006 = "weapon_006";
    public static readonly string weapon007 = "weapon_007";
    public static readonly string weapon008 = "weapon_008";
    public static readonly string weapon009 = "weapon_009";
    public static readonly string weapon010 = "weapon_010";
    public static readonly string weapon011 = "weapon_011";
    public static readonly string weapon012 = "weapon_012";
    public static readonly string weapon013 = "weapon_013";
    public static readonly string weapon014 = "weapon_014";
    public static readonly string weapon015 = "weapon_015";
    public static readonly string weapon016 = "weapon_016";
    public static readonly string weapon017 = "weapon_017";
    public static readonly string weapon018 = "weapon_018";
    public static readonly string weapon019 = "weapon_019";
    public static readonly string weapon020 = "weapon_020";
    public static readonly string weapon021 = "weapon_021";
    public static readonly string weapon022 = "weapon_022";
    public static readonly string weapon023 = "weapon_023";
    public static readonly string weapon024 = "weapon_024";
    public static readonly string weapon025 = "weapon_025";
    public static readonly string weapon026 = "weapon_026";
    public static readonly string weapon027 = "weapon_027";
    public static readonly string weapon028 = "weapon_028";

    // 몬스터 애니메이션
    public static readonly string idle = "idle";
    public static readonly string walk = "walk";
    public static readonly string attack = "attack";
    public static readonly string attack2 = "attack2";
    public static readonly string dead = "dead";

    // 캐릭터 랜덤 뽑기 테스트 용도
    public static List<string> characterSkins = new();
    public static List<string> weaponSkins = new();

    public static void SetSkins()
    {
        characterSkins.Add(body001);
        characterSkins.Add(body024);
        characterSkins.Add(body033);
        characterSkins.Add(body036);
        characterSkins.Add(body038);
        characterSkins.Add(body039);
        characterSkins.Add(body040);
        characterSkins.Add(body043);

        weaponSkins.Add(weapon005);
        weaponSkins.Add(weapon006);
        weaponSkins.Add(weapon007);
        weaponSkins.Add(weapon008);
        weaponSkins.Add(weapon009);
        weaponSkins.Add(weapon010);
        weaponSkins.Add(weapon011);
        weaponSkins.Add(weapon012);
        weaponSkins.Add(weapon013);
        weaponSkins.Add(weapon014);
        weaponSkins.Add(weapon015);
        weaponSkins.Add(weapon016);
        weaponSkins.Add(weapon017);
        weaponSkins.Add(weapon018);
        weaponSkins.Add(weapon019);
        weaponSkins.Add(weapon020);
        weaponSkins.Add(weapon021);
        weaponSkins.Add(weapon022);
        weaponSkins.Add(weapon023);
        weaponSkins.Add(weapon024);
        weaponSkins.Add(weapon025);
        weaponSkins.Add(weapon026);
        weaponSkins.Add(weapon027);
        weaponSkins.Add(weapon028);

    }

    public static ItemType RandomWeaponType()
    {
        int random = UnityEngine.Random.Range(0, MaxWeaponCount);

        switch (random)
        {
            case 0:
                return ItemType.Axe;
            case 1:
                return ItemType.Bow;
            case 2:
                return ItemType.Crossbow;
            case 3:
                return ItemType.Wand;
            case 4:
                return ItemType.Staff;
            default:
                return ItemType.Axe;
        }
    }

    public static Color GetColor(string key)
    {
        switch (key) 
        {
            case "Outline_Blue":
                return Defines.blueColor;
            case "Outline_Green":
                return Defines.greenColor;
            case "Outline_Orange":
                return Defines.orangeColor;
            case "Outline_Purple":
                return Defines.purpleColor;
            case "Outline_Red":
                return Defines.redColor;
            case "Outline_White":
                return Defines.whiteColor;
            case "Outline_Yellow":
                return Defines.yellowColor;

            default:
                return Defines.blueColor;
        }
    }

}
