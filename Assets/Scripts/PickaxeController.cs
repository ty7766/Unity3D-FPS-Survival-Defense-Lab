using System.Collections;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = true;       //무기 활성화 여부

    private void Start()
    {
        //디폴트 무기 생성
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;
    }

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
                //대상이 바위이면 Rock스크립트의 Mining() 호출
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();
                else if (hitInfo.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySoundEffects("Animal_Hit");
                    hitInfo.transform.GetComponent<Pig>().Damage(1, transform.position);
                }
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
