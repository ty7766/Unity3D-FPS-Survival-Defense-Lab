using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 조작")]
    [SerializeField]
    private float walkSpeed;                        //이동 속도(걷기)
    [SerializeField]
    private float runSpeed;                         //이동 속도(달리기)
    [SerializeField]
    private float crouchSpeed;                      //이동 속도(웅크리기)
    [SerializeField]
    private float crouchPosY;                       //웅크리는 정도
    [SerializeField]
    private float jumpForce;                        //점프

    [Header("카메라 조작")]
    [SerializeField]
    private float lookSensitivity;                  //카메라 감도
    [SerializeField]
    private float cameraRotationLimit;              //카메라 회전 제한

    private float currentCameraRotationX =0f;       //현재 카메라 회전량
    private float applySpeed;                       //통합 속도

    private float originPosY;                       //기본 Y 값
    private float applyCrouchPosY;                  //통합 웅크리기

    //상태변수
    private bool isGround = true;
    public bool isRun = false;
    private bool isCrouch = false;
    private bool isWalk = false;

    //움직임 체크
    private Vector3 lastPos;                        //전 프레임의 플레이어 위치(움직임 체크용)

    [Header("연결 컴포넌트")]
    public Camera cam;
    public CrossHair crossHair;

    private GunController gunController;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private Animator animator;

    //초기화
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();        //Camera가 2개이므로 태그를 이용하여 찾음.
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        gunController = FindAnyObjectByType<GunController>();
        crossHair = FindAnyObjectByType<CrossHair>();

        applySpeed = walkSpeed;                     //기본 상태 초기화 (걷기)

        originPosY = cam.transform.localPosition.y;
        applyCrouchPosY = originPosY;               //웅크리기 정도 초기화
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        CamRotation();
        CharacterRotation();
    }

    //-------------- 플레이어 이동 메소드 --------------------
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");     //횡이동
        float moveDirectionZ = Input.GetAxisRaw("Vertical");       //종이동

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 v = (moveHorizontal + moveVertical).normalized * applySpeed;     //normalized : 벡터 정규화

        myRigid.MovePosition(transform.position + v * Time.deltaTime);
    }

    //------------------ 플레이어 움직임 확인 --------------------
    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround)
        {
            //현재 프레임의 플레이어 위치와 이전 프레임의 플레이어 위치 비교
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            crossHair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    //---------------- 상하 카메라 회전 메소드 ------------------
    private void CamRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float camRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= camRotationX;                                                                             // +=는 마우스 반전
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);            //카메라 각도의 최소/최댓값 적용

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);                                       //마우스 종 이동에 따른 카메라 회전
    }

    //---------------- 좌우 캐릭터 회전 메소드 -------------------
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 charRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        //오일러 값인 charRotationY를 Quaternion으로 변환
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(charRotationY));
    }

    //------------------- 플레이어 걷기-달리기 상태 확인 ----------------
    private void TryRun()
    {
        //LS를 누르고 있으면 달리기 전환, 떼면 걷기 전환
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    //--------------------- 플레이어 걷기-달리기 전환 메소드 -----------------------
    private void Running()
    {
        //달리기를 누르면 웅크리기 해제
        if (isCrouch)
            Crouch();

        //달릴 때 정조준 해제
        gunController.CancelFineSight();

        isRun = true;
        crossHair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        crossHair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    //----------------------- 플레이어 점프 메소드 ----------------------------
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        //점프를 누르면 웅크리기 해제
        if (isCrouch)
            Crouch();

        myRigid.linearVelocity = transform.up * jumpForce;
    }
    private void IsGround()
    {
        //캡슐콜라이더 영역의 y길이 반을 레이저로 쏘기
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        crossHair.JumpAnimation(!isGround);
    }


    //---------------- 플레이어 웅크리기 메소드 ------------------------
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        crossHair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        //카메라의 Y위치만 웅크리기 정도만큼 변경
        StartCoroutine(CrouchCoroutine());
    }

    //------------------------ 웅크리기 전환 속도 조정 --------------------------
    IEnumerator CrouchCoroutine()
    {
        //현재 Y값을 설정 - 기본 Y값으로 초기화
        //posY가 웅크리는 Y값이 될 때까지 코루틴으로 실행
        float posY = cam.transform.localPosition.y;
        int count = 0; 

        while(posY != applyCrouchPosY)
        {
            count++;
            //웅크리는 시간 보간
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.4f);
            cam.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        //Lerp를 사용하면 목적 좌표까지 딱 떨어지지 않게 끝남 -> 부드럽게 적용시키기 위해 어느정도 반복 후 원하는 목적 좌표 값으로 변경
        cam.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
}
