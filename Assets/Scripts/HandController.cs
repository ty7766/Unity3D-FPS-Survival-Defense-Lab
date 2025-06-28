using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivate = false;       //���� Ȱ��ȭ ����

    [Header("���� ���� Hand�� Ÿ�� ����")]
    public Hand currentHand;

    private bool isAttack;                  //����������
    private bool isSwing;                   //���� �ֵθ�����
    private RaycastHit hitInfo;             //�÷��̾�� ��ȣ�ۿ��ϴ� ������Ʈ

    void Update()
    {
        //���� ���Ⱑ "��"�� ��� �Ǽ� ��ȣ�ۿ� ����
        if (isActivate)
            TryAttack();
    }
    
    //------------------- �÷��̾� ���� ---------------------------
    private void TryAttack()
    {
        //��Ŭ���ϸ� ��ȣ�ۿ�
        if (Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    //---------------------------- �÷��̾� ���� ��� �� ������ ���� -------------------------------
    IEnumerator AttackCoroutine()
    {
        //���� �غ�
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");                      //���� ��� ����
        yield return new WaitForSeconds(currentHand.attackDelayA);  //���� Ȱ��ȭ �ð�
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);  //���� �� �ߺ� ���� ����
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
        
        //���� ���� ����
        //���� ���콺 �Է� ���
    }


    //--------------------- ���� �� �϶� ������ ������Ʈ üũ -------------------------
    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;            //�ߺ� ���� ����
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    //--------------------- ���� �� ������Ʈ ��ȯ -----------------------------
    private bool CheckObject()
    {
        //�÷��̾�� ���� ���̷� ������Ʈ ���� �� ������Ʈ ���� ��ȯ
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    //----------------------------- ���� ��ü ----------------------------
    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);        //���� ���� �Ⱥ��̰� �ϱ�

        currentHand = _hand;                                                  //���� ���⸦ ���� ����� ����
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>(); //���� ������ ������Ʈ ����
        WeaponManager.currentWeaponAnimator = currentHand.anim;              //���� ������ �ִϸ��̼� ����

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);                              //���� ���� ���̰� �ϱ�

        isActivate = true;
    }
}
