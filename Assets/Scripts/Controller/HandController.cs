using System.Collections;
using UnityEngine;

//CloseWeaponController를 상속
public class HandController : CloseWeaponController
{
    public static bool isActivate = false;       //무기 활성화 여부

    void Update()
    {
        //현재 무기가 "총"인 경우 맨손 상호작용 금지
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;            //중복 공격 방지
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    //부모클래스의 virtual method를 활용하여 재정의
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
