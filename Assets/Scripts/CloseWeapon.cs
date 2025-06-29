using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public string closeWeaponName;      //���� ���� �̸�
    public float range;                 //���� ����
    public float workSpeed;             //�۾� �ӵ�
    public float attackDelay;           //���� ������
    public float attackDelayA;          //���� Ȱ��ȭ ������
    public float attackDelayB;          //���� ��Ȱ��ȭ ������ (����� ��Ȱ��ȭ)
    public int damage;                  //���� �����

    //�� ���⸦ �����Ͽ� �� �Ӽ����� ���
    [Header("���� ����")]
    public bool isAxe;          //��������
    public bool isPickaxe;      //�������
    public bool isHand;         //�Ǽ�����

    [Header("�ʿ� ������Ʈ")]
    public Animator anim;
}
