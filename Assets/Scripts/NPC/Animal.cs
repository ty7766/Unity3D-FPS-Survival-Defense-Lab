using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("���� �Ӽ�")]
    [SerializeField]
    protected string animalName;          //���� �̸�
    [SerializeField]
    protected int hp;                     //ü��
    [SerializeField]
    protected float walkSpeed;            //�̵� �ӵ� (�ȱ�)
    [SerializeField]
    protected float walkTime;             //�ȱ� �ð�
    [SerializeField]
    protected float runSpeed;             //�̵� �ӵ� (�ٱ�)
    [SerializeField]
    protected float runTime;              //�ٱ� �ð�
    [SerializeField]
    protected float waitTime;             //��� �ð�
    [SerializeField]
    protected float turningSpeed;         //ȸ�� �ӵ�

    [Header("���� ������Ʈ")]
    [SerializeField]
    protected AudioClip[] soundIdle;      //�⺻ ���� ����
    [SerializeField]
    protected AudioClip soundHurt;        //�ǰ� ���� ����
    [SerializeField]
    protected AudioClip soundDead;        //��� ���� ����
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rd;
    [SerializeField]
    protected BoxCollider boxCollider;

    protected bool isWalking;             //�ȴ��� �ƴ���
    protected bool isAction;              //�ൿ������ �ƴ���
    protected bool isRunning;             //�ٴ��� �ƴ���
    protected bool isDead;                //�׾����� �ƴ���

    protected float currentTime;
    protected float applySpeed;

    protected Vector3 direc;              //�̵� ����
    protected AudioSource audioSource;

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
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            rd.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }


    //------------------------------------------ ȸ�� �޼ҵ� ---------------------------------------
    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direc.y, 0f), turningSpeed);
            rd.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    //------------------------------------------ �ǰ� �޼ҵ� -------------------------------------
    public virtual void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {
            hp -= _dmg;

            if (hp <= 0)
            {
                Dead();
                return;
            }

            PlaySound(soundHurt);
            anim.SetTrigger("Hurt");
        }
    }

    //------------------------------------------ ��� �޼ҵ� -------------------------------------
    protected void Dead()
    {
        PlaySound(soundDead);
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
    protected virtual void ResetAction()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direc.Set(0f, Random.Range(0f, 360f), 0f);
    }

    //-------------------���� �׼� �޼ҵ�-----------------------
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("�ȱ�");
    }


    //------------------- ���� ��� ---------------------------
    protected void PlaySound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    protected void RandomSound()
    {
        int random = Random.Range(0, 3);        //3���� ���� ����
        PlaySound(soundIdle[random]);        //Idle ���� ���� ���
    }
}
