using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    [Header("플레이어 공격")]
    public string closeWeaponName;      //근접 무기 이름
    public float range;                 //공격 범위
    public float workSpeed;             //작업 속도
    public float attackDelay;           //공격 딜레이
    public float attackDelayA;          //공격 활성화 딜레이
    public float attackDelayB;          //공격 비활성화 딜레이 (대미지 비활성화)
    public int damage;                  //공격 대미지

    //각 무기를 통합하여 위 속성들을 상속
    [Header("무기 유형")]
    public bool isAxe;          //도끼인지
    public bool isPickaxe;      //곡괭이인지
    public bool isHand;         //맨손인지

    [Header("필요 컴포넌트")]
    public Animator anim;
}
