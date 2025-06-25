using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("플레이어 공격")]
    public string handName;             //팔 이름
    public float range;                 //공격 범위
    public float workSpeed;             //작업 속도
    public float attackDelay;           //공격 딜레이
    public float attackDelayA;          //공격 활성화 딜레이
    public float attackDelayB;          //공격 비활성화 딜레이 (대미지 비활성화)
    public int damage;                  //공격 대미지

    [Header("필요 컴포넌트")]
    public Animator anim;
}
