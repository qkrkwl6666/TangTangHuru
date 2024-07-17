using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Defines
{
    public static readonly string exp = "Exp";
    public static readonly string normalBullet = "NormalBullet";
    public static readonly string snailBullet = "SnailBullet";
    public static readonly string treasure = "Treasure";

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

    // 캐릭터 랜덤 뽑기 테스트 용도
    public static List<string> characterSkins = new ();
    public static List<string> weaponSkins = new ();

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
