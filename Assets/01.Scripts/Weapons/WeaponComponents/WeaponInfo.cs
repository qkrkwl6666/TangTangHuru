
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    //무기 수치를 모두 들고 있는 클래스.
    //레벨업시, 강화시 오직 이 클래스와 상호작용한다.

    public int weapon_Level;
    public float weapon_Damage;
    public float weapon_Range;
    public float weapon_Speed; //무기 발사 속도
    public float weapon_CoolDown; //다음 공격까지의 간격

    public int weapon_BurstCount; //연속 발사 개수
    public float weapon_BurstRate; //연속 발사 간격
    public int weapon_pierceCount; //관통 개수

    public float weapon_LifeTime; //무기 유지시간
}
