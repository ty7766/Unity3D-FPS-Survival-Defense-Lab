using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun 기본 속성")]
    public string gunName;          //총 이름
    public float range;             //사정거리
    public float accuracy;          //정확도 (집탄율)
    public float fireRate;          //연사속도
    public float reloadTime;        //재장전속도
    public int damage;              //대미지
    public float retroActionForce;  //반동력
    public float retroActionFineSightForce;     //정조준 시 반동력
    public Vector3 fineSightOriginPos;      //정조준 시 총 위치

    [Header("Bullet 기본 속성")]
    public int reloadBulletCount;   //총알 재장전 개수
    public int currentBulletCount;  //현재 총알 개수
    public int maxBulletCount;      //최대 소유 가능 총알 개수
    public int carryBulletCount;        //현재 소유 총알 개수

    [Header("연결 컴포넌트")]
    public Animator anim;                   //총 애니메이션
    public ParticleSystem muzzleFlash;      //총구 섬광 이펙트
    public AudioClip fireSound;             //발사음
}
