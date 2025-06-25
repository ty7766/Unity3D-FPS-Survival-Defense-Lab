using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public string handName;             //�� �̸�
    public float range;                 //���� ����
    public float workSpeed;             //�۾� �ӵ�
    public float attackDelay;           //���� ������
    public float attackDelayA;          //���� Ȱ��ȭ ������
    public float attackDelayB;          //���� ��Ȱ��ȭ ������ (����� ��Ȱ��ȭ)
    public int damage;                  //���� �����

    [Header("�ʿ� ������Ʈ")]
    public Animator anim;
}
