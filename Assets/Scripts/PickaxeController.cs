using System.Collections;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = true;       //���� Ȱ��ȭ ����

    private void Start()
    {
        //����Ʈ ���� ����
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;
    }

    void Update()
    {
        //���� ���Ⱑ "��"�� ��� �Ǽ� ��ȣ�ۿ� ����
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //����� �����̸�
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<Rock>().Mining();

                //����� WeakAnimal�̸� 
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySoundEffects("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damage(1, transform.position);
                }

                isSwing = false;            //�ߺ� ���� ����
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    //�θ�Ŭ������ virtual method�� Ȱ���Ͽ� ������
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
