using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [Header("���� �Ӽ�")]
    [SerializeField]
    private string animalName;          //���� �̸�
    [SerializeField]
    private int hp;                     //ü��
    [SerializeField]
    private float walkSpeed;            //�̵� �ӵ� (�ȱ�)
    [SerializeField]
    private float walkTime;             //�ȱ� �ð�
    [SerializeField]
    private float waitTime;             //��� �ð�

    [Header("���� ������Ʈ")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rd;
    [SerializeField]
    private BoxCollider boxCollider;

    private bool isWalking;             //�ȴ��� �ƴ���
    private bool isAction;              //�ൿ������ �ƴ���

    private float currentTime;

    private Vector3 direc;              //�̵� ����

    //�ʱ�ȭ
    void Start()
    {
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        Move();
        Rotation();
        ElapseTime();
    }

    //------------------------------------------ �̵� �޼ҵ� ---------------------------------------
    private void Move()
    {
        if (isWalking)
        {
            rd.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }
    //------------------------------------------ ȸ�� �޼ҵ� ---------------------------------------
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direc, 0.01f);
            rd.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    //------------------------------------ ���� �ൿ �޼ҵ� -----------------------------------
    //�Ҵ�� �׼� �ð��� ������ ���� �׼����� �Ѿ
    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ResetAction();
        }
    }
    //��� �׼� �ʱ�ȭ�ϰ� �ٽ� �׼� ����
    private void ResetAction()
    {
        isWalking = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        direc.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    //�׼� �б�
    private void RandomAction()
    {
        isAction = true;

        int _random = Random.Range(0, 4);       //0,1,2,3 ����

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }

    //-------------------���� �׼� �޼ҵ�-----------------------
    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("���");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("Ǯ���");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("�θ���");
    }
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        currentTime = walkTime;
        Debug.Log("�ȱ�");
    }
}
