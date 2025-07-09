using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Pig : MonoBehaviour
{
    [Header("돼지 속성")]
    [SerializeField]
    private string animalName;          //동물 이름
    [SerializeField]
    private int hp;                     //체력
    [SerializeField]
    private float walkSpeed;            //이동 속도 (걷기)
    [SerializeField]
    private float walkTime;             //걷기 시간
    [SerializeField]
    private float runSpeed;             //이동 속도 (뛰기)
    [SerializeField]
    private float runTime;              //뛰기 시간
    [SerializeField]
    private float waitTime;             //대기 시간

    [Header("연결 컴포넌트")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rd;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private AudioClip[] soundPigIdle;      //기본 상태 음성
    [SerializeField]
    private AudioClip soundPigHurt;        //피격 상태 음성
    [SerializeField]
    private AudioClip soundPigDead;        //사망 상태 음성

    private bool isWalking;             //걷는지 아닌지
    private bool isAction;              //행동중인지 아닌지
    private bool isRunning;             //뛰는지 아닌지
    private bool isDead;                //죽었는지 아닌지

    private float currentTime;
    private float applySpeed;

    private Vector3 direc;              //이동 방향
    private AudioSource audioSource;

    //초기화
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

    //------------------------------------------ 이동 메소드 ---------------------------------------
    private void Move()
    {
        if (isWalking || isRunning)
        {
            rd.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }


    //------------------------------------------ 회전 메소드 ---------------------------------------
    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direc.y, 0f), 0.01f);
            rd.MoveRotation(Quaternion.Euler(_rotation));
        }
    }


    //------------------------------------------ 뛰기 메소드 ---------------------------------------
    //공격자에게 공격을 받은 순간 공격자의 반대 방향으로 뛰기
    public void Run(Vector3 _targetPos)
    {
        //뛰는 방향을 공격자의 반대방향으로 설정
        direc = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

    }
    

    //------------------------------------------ 피격 메소드 -------------------------------------
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

    //------------------------------------------ 사망 메소드 -------------------------------------
    public void Dead()
    {
        PlaySound(soundPigDead);
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
    //액션 분기
    private void RandomAction()
    {
        RandomSound();

        int _random = Random.Range(0, 4);       //0,1,2,3 랜덤

        if (_random == 0)
            Wait();
        else if (_random == 1)
            Eat();
        else if (_random == 2)
            Peek();
        else if (_random == 3)
            TryWalk();
    }


    //-------------------실제 액션 메소드-----------------------
    private void Wait()
    {
        currentTime = waitTime;
        Debug.Log("대기");
    }
    private void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
        Debug.Log("풀뜯기");
    }
    private void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
        Debug.Log("두리번");
    }
    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        currentTime = walkTime;
        applySpeed = walkSpeed;
        Debug.Log("걷기");
    }


    //------------------- 사운드 재생 ---------------------------
    private void PlaySound(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    private void RandomSound()
    {
        int random = Random.Range(0, 3);        //3개의 음성 포함
        PlaySound(soundPigIdle[random]);        //Idle 사운드 랜덤 재생
    }
}
