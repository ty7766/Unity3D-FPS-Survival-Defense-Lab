using System.Collections;
using System.Runtime.ExceptionServices;
using UnityEngine;

//�߻�Ŭ������ ������Ʈ�� ������� �� ����
public abstract class CloseWeaponController : MonoBehaviour
{

    [Header("���� ���� Hand�� Ÿ�� ����")]
    public CloseWeapon currentCloseWeapon;

    [Header("���� ������Ʈ")]
    [SerializeField]
    protected LayerMask layerMask;

    protected bool isAttack;                  //����������
    protected bool isSwing;                   //���� �ֵθ�����
    protected RaycastHit hitInfo;             //�÷��̾�� ��ȣ�ۿ��ϴ� ������Ʈ

    //---------------------------- �÷��̾� ���� ---------------------------
    protected void TryAttack()
    {
        if(!Inventory.inventoryActivated)
        {
            //��Ŭ���ϸ� ��ȣ�ۿ�
            if (Input.GetButton("Fire1"))
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());
                }
            }
        }
    }



    //------------------ �÷��̾� ���� ��� �� ������ ���� ----------------
    protected IEnumerator AttackCoroutine()
    {
        //���� �غ�
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");                      //���� ��� ����
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);  //���� Ȱ��ȭ �ð�
        isSwing = true;

        //���� Ȱ��ȭ ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);  //���� �� �ߺ� ���� ����
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;

        //���� ���� ����
        //���� ���콺 �Է� ���
    }



    //--------------------- ���� �� ������Ʈ ��ȯ -------------------------
    protected bool CheckObject()
    {
        //�÷��̾�� ���� ���̷� ������Ʈ ���� �� ������Ʈ ���� ��ȯ
        //fix. �þ߰� �������� ���� �÷��̾��� ���̾ Player�� �Ǿ� CloseWeapon���� �ڱ� �ڽ��� �ǰݵǴ� �� ����
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            return true;
        }
        return false;
    }


    //-------------------------- ���� ���� ��ü ---------------------------
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);                //���� ���� �Ⱥ��̰� �ϱ�

        currentCloseWeapon = _closeWeapon;                                          //���� ���⸦ ���� ����� ����
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>(); //���� ������ ������Ʈ ����
        WeaponManager.currentWeaponAnimator = currentCloseWeapon.anim;              //���� ������ �ִϸ��̼� ����

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);                              //���� ���� ���̰� �ϱ�
    }


    //--------------------- ���� �� �϶� ������ ������Ʈ üũ -------------------------
    protected abstract IEnumerator HitCoroutine();
}
