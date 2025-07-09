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
    private float runSpeed;             //�̵� �ӵ� (�ٱ�)
    [SerializeField]
    private float runTime;              //�ٱ� �ð�
    [SerializeField]
    private float waitTime;             //��� �ð�

    [Header("���� ������Ʈ")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rd;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private AudioClip[] soundPigIdle;      //�⺻ ���� ����
    [SerializeField]
    private AudioClip soundPigHurt;        //�ǰ� ���� ����
    [SerializeField]
    private AudioClip soundPigDead;        //��� ���� ����

    private bool isWalking;             //�ȴ��� �ƴ���
    private bool isAction;              //�ൿ������ �ƴ���
    private bool isRunning;             //�ٴ��� �ƴ���
    private bool isDead;                //�׾����� �ƴ���

    private float currentTime;
    private float applySpeed;

    private Vector3 direc;              //�̵� ����
    private AudioSource audioSource;

    //�ʱ�ȭ
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    //------------------------------------------ �̵� �޼ҵ� ---------------------------------------
    private void Move()
    {
        if (isWalking || isRunning)
        {
            rd.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }


    //------------------------------------------ ȸ�� �޼ҵ� ---------------------------------------
    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direc.y, 0f), 0.01f);
            rd.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    //------------------------------------------ �ٱ� �޼ҵ� ---------------------------------------
    //�����ڿ��� ������ ���� ���� �������� �ݴ� �������� �ٱ�
    public void Run(Vector3 _targetPos)
    {
        //�ٴ� ������ �������� �ݴ�������� ����
        direc = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

    }
    

    //------------------------------------------ �ǰ� �޼ҵ� -------------------------------------
    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySound(soundPigHurt);
            anim.SetTrigger("Hurt");
            Run(_targetPos);
        }
    }

    //------------------------------------------ ��� �޼ҵ� -------------------------------------
    public void Dead()
    {
        PlaySound(soundPigDead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
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
        isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direc.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    //�׼� �б�
    private void RandomAction()
    {
        RandomSound();

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
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");
    }


    //------------------- ���� ��� ---------------------------
    private void PlaySound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    private void RandomSound()
    {
        int random = Random.Range(0, 3);        //3���� ���� ����
        PlaySound(soundPigIdle[random]);        //Idle ���� ���� ���
    }
}
