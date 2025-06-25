using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [Header("현재 장착 Hand형 타입 무기")]
    public Hand currentHand;

    private bool isAttack;                  //공격중인지
    private bool isSwing;                   //팔을 휘두르는지
    private RaycastHit hitInfo;             //플레이어와 상호작용하는 오브젝트

    void Update()
    {
        TryAttack();
    }
    
    //------------------- 플레이어 공격 ---------------------------
    private void TryAttack()
    {
        //좌클릭하면 상호작용
        if (Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    //---------------------------- 플레이어 공격 모션 및 딜레이 적용 -------------------------------
    IEnumerator AttackCoroutine()
    {
        //공격 준비
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");                      //공격 모션 실행
        yield return new WaitForSeconds(currentHand.attackDelayA);  //공격 활성화 시간
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);  //공격 중 중복 공격 방지
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
        
        //공격 종료 시점
        //다음 마우스 입력 대기
    }


    //--------------------- 공격 중 일때 공격한 오브젝트 체크 -------------------------
    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;            //중복 공격 방지
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    //--------------------- 접촉 한 오브젝트 반환 -----------------------------
    private bool CheckObject()
    {
        //플레이어에서 나온 레이로 오브젝트 접촉 후 오브젝트 정보 반환
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
