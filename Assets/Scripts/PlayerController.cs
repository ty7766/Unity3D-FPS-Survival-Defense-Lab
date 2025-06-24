using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;                        //�̵� �ӵ�(�ȱ�)
    [SerializeField]
    private float lookSensitivity;                  //ī�޶� ����
    [SerializeField]
    private float cameraRotationLimit;              //ī�޶� ȸ�� ����

    private float currentCameraRotationX =0f;       //���� ī�޶� ȸ����
    private Rigidbody myRigid;


    public Camera cam;

    //�ʱ�ȭ
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        cam = FindAnyObjectByType<Camera>();        //�� �� ī�޶�� 1���̹Ƿ� Any ����
    }

    void Update()
    {
        Move();
        CamRotation();
        CharacterRotation();
    }

    //-------------- �÷��̾� �̵� �޼ҵ�----------
    private void Move()
    {
        float moveDirectionX = Input.GetAxisRaw("Horizontal");     //Ⱦ�̵�
        float moveDirectionZ = Input.GetAxisRaw("Vertical");       //���̵�

        Vector3 moveHorizontal = transform.right * moveDirectionX;
        Vector3 moveVertical = transform.forward * moveDirectionZ;

        Vector3 v = (moveHorizontal + moveVertical).normalized * walkSpeed;     //normalized : ���� ����ȭ

        myRigid.MovePosition(transform.position + v * Time.deltaTime);
    }

    //---------------- ���� ī�޶� ȸ�� �޼ҵ� ------------------
    private void CamRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float camRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= camRotationX;                                                                             // +=�� ���콺 ����
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);            //ī�޶� ������ �ּ�/�ִ� ����

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);                                       //���콺 �� �̵��� ���� ī�޶� ȸ��
    }

    //---------------- �¿� ĳ���� ȸ�� �޼ҵ� -------------------
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 charRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        //���Ϸ� ���� charRotationY�� Quaternion���� ��ȯ
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(charRotationY));
    }
}
