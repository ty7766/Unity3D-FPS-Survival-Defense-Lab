using System.Collections;
using UnityEngine;

//CloseWeaponController�� ���
public class HandController : CloseWeaponController
{
    public static bool isActivate = false;       //���� Ȱ��ȭ ����

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
