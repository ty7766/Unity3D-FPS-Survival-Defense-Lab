using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;                        //이동 속도(걷기)
    [SerializeField]
    private float lookSensitivity;                  //카메라 감도
    [SerializeField]
    private float cameraRotationLimit;              //카메라 회전 제한

    private float currentCameraRotationX =0f;       //현재 카메라 회전량
    private Rigidbody myRigid;


    public Camera cam;

    //초기화
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        cam = FindAnyObjectByType<Camera>();        //씬 내 카메라는 1개이므로 Any 선택
    }

    void Update()
    {
        Move();
        CamRotation();
        CharacterRotation();
    }

    //-------------- 플레이어 이동 메소드----------
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");     //횡이동
        float moveDirectionZ = Input.GetAxisRaw("Vertical");       //종이동

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 v = (moveHorizontal + moveVertical).normalized * walkSpeed;     //normalized : 벡터 정규화

        myRigid.MovePosition(transform.position + v * Time.deltaTime);
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
}
