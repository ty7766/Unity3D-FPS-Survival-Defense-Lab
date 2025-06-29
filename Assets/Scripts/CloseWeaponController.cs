using System.Collections;
using UnityEngine;

//추상클래스는 컴포넌트로 집어넣을 수 없음
public abstract class CloseWeaponController : MonoBehaviour
{

    [Header("현재 장착 Hand형 타입 무기")]
    public CloseWeapon currentCloseWeapon;

    protected bool isAttack;                  //공격중인지
    protected bool isSwing;                   //팔을 휘두르는지
    protected RaycastHit hitInfo;             //플레이어와 상호작용하는 오브젝트

    //------------------- 플레이어 공격 ---------------------------
    protected void TryAttack()
    {
        //좌클릭하면 상호작용
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    //---------------------------- 플레이어 공격 모션 및 딜레이 적용 -------------------------------
    protected IEnumerator AttackCoroutine()
    {
        //공격 준비
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");                      //공격 모션 실행
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);  //공격 활성화 시간
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);  //공격 중 중복 공격 방지
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;

        //공격 종료 시점
        //다음 마우스 입력 대기
    }

    //--------------------- 접촉 한 오브젝트 반환 -----------------------------
    protected bool CheckObject()
    {
        //플레이어에서 나온 레이로 오브젝트 접촉 후 오브젝트 정보 반환
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    //-------------------------- 근접 무기 교체 ---------------------------
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);                //현재 무기 안보이게 하기

        currentCloseWeapon = _closeWeapon;                                          //다음 무기를 현재 무기로 설정
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); //다음 무기의 오브젝트 적용
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;              //다음 무기의 애니메이션 적용

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);                              //다음 무기 보이게 하기
    }

    //--------------------- 공격 중 일때 공격한 오브젝트 체크 -------------------------
    protected abstract IEnumerator HitCoroutine();
}
