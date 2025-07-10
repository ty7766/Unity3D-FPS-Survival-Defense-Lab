using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [Header("동물 속성")]
    [SerializeField]
    protected string animalName;          //동물 이름
    [SerializeField]
    protected int hp;                     //체력
    [SerializeField]
    protected float walkSpeed;            //이동 속도 (걷기)
    [SerializeField]
    protected float walkTime;             //걷기 시간
    [SerializeField]
    protected float runSpeed;             //이동 속도 (뛰기)
    [SerializeField]
    protected float runTime;              //뛰기 시간
    [SerializeField]
    protected float waitTime;             //대기 시간

    [Header("연결 컴포넌트")]
    [SerializeField]
    protected AudioClip[] soundIdle;      //기본 상태 음성
    [SerializeField]
    protected AudioClip soundHurt;        //피격 상태 음성
    [SerializeField]
    protected AudioClip soundDead;        //사망 상태 음성
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected Rigidbody rd;
    [SerializeField]
    protected BoxCollider boxCollider;

    protected bool isWalking;             //걷는지 아닌지
    protected bool isAction;              //행동중인지 아닌지
    protected bool isRunning;             //뛰는지 아닌지
    protected bool isDead;                //죽었는지 아닌지

    protected float currentTime;

    protected Vector3 dest;              //목적지
    protected AudioSource audioSource;
    protected NavMeshAgent nav;

    //초기화
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        currentTime = waitTime;
        isAction = true;
    }

    void Update()
    {
        if (!isDead)
        {
            Move();
            ElapseTime();
        }
    }

    //------------------------------------------ 이동 메소드 ---------------------------------------
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rd.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
            nav.SetDestination(transform.position + dest * 5f);
        }
    }


    //------------------------------------------ 피격 메소드 -------------------------------------
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

    //------------------------------------------ 사망 메소드 -------------------------------------
    protected void Dead()
    {
        PlaySound(soundDead);
        isWalking = false;
        isRunning = false;
        isDead = true;
        anim.SetTrigger("Dead");
    }


    //------------------------------------ 랜덤 행동 메소드 -----------------------------------
    //할당된 액션 시간이 끝나면 다음 액션으로 넘어감
    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            ResetAction();
        }
    }

    //모든 액션 초기화하고 다시 액션 시작
    protected virtual void ResetAction()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        nav.speed = walkSpeed;
        nav.ResetPath();
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        dest.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    //-------------------실제 액션 메소드-----------------------
    protected void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        currentTime = walkTime;
        nav.speed = walkSpeed;
        Debug.Log("걷기");
    }


    //------------------- 사운드 재생 ---------------------------
    protected void PlaySound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    protected void RandomSound()
    {
        int random = Random.Range(0, 3);        //3개의 음성 포함
        PlaySound(soundIdle[random]);        //Idle 사운드 랜덤 재생
    }
}
