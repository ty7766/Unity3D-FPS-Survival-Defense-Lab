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
    private float waitTime;             //대기 시간

    [Header("연결 컴포넌트")]
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rd;
    [SerializeField]
    private BoxCollider boxCollider;

    private bool isWalking;             //걷는지 아닌지
    private bool isAction;              //행동중인지 아닌지

    private float currentTime;

    private Vector3 direc;              //이동 방향

    //초기화
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

    //------------------------------------------ 이동 메소드 ---------------------------------------
    private void Move()
    {
        if (isWalking)
        {
            rd.MovePosition(transform.position + transform.forward * walkSpeed * Time.deltaTime);
        }
    }
    //------------------------------------------ 회전 메소드 ---------------------------------------
    private void Rotation()
    {
        if (isWalking)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direc, 0.01f);
            rd.MoveRotation(Quaternion.Euler(_rotation));
        }
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
        anim.SetBool("Walking", isWalking);
        direc.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }
    //액션 분기
    private void RandomAction()
    {
        isAction = true;

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
        Debug.Log("걷기");
    }
}
