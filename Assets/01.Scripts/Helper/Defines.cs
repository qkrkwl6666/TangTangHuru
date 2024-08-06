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

    // �ƿ����̳� �÷�
    public static readonly Color blueColor = new Color(2 / 255f, 19 / 255f, 52 / 255f);
    public static readonly Color greenColor = new Color(20 / 255f, 46 / 255f, 34 / 255f);
    public static readonly Color orangeColor = new Color(45 / 255f, 30 / 255f, 18 / 255f);
    public static readonly Color purpleColor = new Color(33 / 255f, 13 / 255f, 52 / 255f);
    public static readonly Color redColor = new Color(39 / 255f, 10 / 255f, 8 / 255f);
    public static readonly Color whiteColor = Color.white;
    public static readonly Color yellowColor = new Color(40 / 255f, 36 / 255f, 29 / 255f);

    //// �ƿ� ���̳� 
    //public static readonly string outlineBlue = "Outline_Blue";
    //public static readonly string outlineGreen = "Outline_Green";
    //public static readonly string outlineOrange = "Outline_Orange";
    //public static readonly string outlinePurple = "Outline_Purple";
    //public static readonly string outlineRed = "Outline_Red";
    //public static readonly string outlineWhite = "Outline_White";
    //public static readonly string outlineYellow = "Outline_Yellow";

    public static readonly string itemSlot = "ItemSlot";

    // ������ ĳ���� ��Ų
    public static readonly string body001 = "body_001";
    public static readonly string body024 = "body_024";
    public static readonly string body033 = "body_033";
    public static readonly string body036 = "body_036";
    public static readonly string body038 = "body_038";
    public static readonly string body039 = "body_039";
    public static readonly string body040 = "body_040";
    public static readonly string body043 = "body_043";

    // ������ ���� 
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

    // ���� �ִϸ��̼�
    public static readonly string idle = "idle";
    public static readonly string walk = "walk";
    public static readonly string attack = "attack";
    public static readonly string attack2 = "attack2";
    public static readonly string dead = "dead";

    // ĳ���� ���� �̱� �׽�Ʈ �뵵
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


}
