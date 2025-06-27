using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun �⺻ �Ӽ�")]
    public string gunName;          //�� �̸�
    public float range;             //�����Ÿ�
    public float accuracy;          //��Ȯ�� (��ź��)
    public float fireRate;          //����ӵ�
    public float reloadTime;        //�������ӵ�
    public int damage;              //�����
    public float retroActionForce;  //�ݵ���
    public float retroActionFineSightForce;     //������ �� �ݵ���
    public Vector3 fineSightOriginPos;      //������ �� �� ��ġ

    [Header("Bullet �⺻ �Ӽ�")]
    public int reloadBulletCount;   //�Ѿ� ������ ����
    public int currentBulletCount;  //���� �Ѿ� ����
    public int maxBulletCount;      //�ִ� ���� ���� �Ѿ� ����
    public int carryBulletCount;        //���� ���� �Ѿ� ����

    [Header("���� ������Ʈ")]
    public Animator anim;                   //�� �ִϸ��̼�
    public ParticleSystem muzzleFlash;      //�ѱ� ���� ����Ʈ
    public AudioClip fireSound;             //�߻���
}
