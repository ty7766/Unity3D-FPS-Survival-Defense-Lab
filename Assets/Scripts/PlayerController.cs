using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    [SerializeField]
    private float walkSpeed;                        //�̵� �ӵ�(�ȱ�)
    [SerializeField]
    private float runSpeed;                         //�̵� �ӵ�(�޸���)
    [SerializeField]
    private float crouchSpeed;                      //�̵� �ӵ�(��ũ����)
    [SerializeField]
    private float crouchPosY;                       //��ũ���� ����
    [SerializeField]
    private float jumpForce;                        //����

    [Header("ī�޶� ����")]
    [SerializeField]
    private float lookSensitivity;                  //ī�޶� ����
    [SerializeField]
    private float cameraRotationLimit;              //ī�޶� ȸ�� ����

    private float currentCameraRotationX =0f;       //���� ī�޶� ȸ����
    private float applySpeed;                       //���� �ӵ�

    private float originPosY;                       //�⺻ Y ��
    private float applyCrouchPosY;                  //���� ��ũ����

    private bool isGround = true;
    private bool isRun = false;
    private bool isCrouch = false;

    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private Animator animator;
    public Camera cam;

    //�ʱ�ȭ
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        cam = FindAnyObjectByType<Camera>();        //�� �� ī�޶�� 1���̹Ƿ� Any ����
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();

        applySpeed = walkSpeed;                     //�⺻ ���� �ʱ�ȭ (�ȱ�)

        originPosY = cam.transform.localPosition.y;
        applyCrouchPosY = originPosY;               //��ũ���� ���� �ʱ�ȭ
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
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

        Vector3 v = (moveHorizontal + moveVertical).normalized * applySpeed;     //normalized : ���� ����ȭ

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

    //------------------- �÷��̾� �ȱ�-�޸��� ���� Ȯ�� ----------------
    private void TryRun()
    {
        //LS�� ������ ������ �޸��� ��ȯ, ���� �ȱ� ��ȯ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walk();
        }
    }

    //--------------------- �÷��̾� �ȱ�-�޸��� ��ȯ �޼ҵ� -----------------------
    private void Running()
    {
        //�޸��⸦ ������ ��ũ���� ����
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
    }
    private void Walk()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //----------------------- �÷��̾� ���� �޼ҵ� ----------------------------
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        //������ ������ ��ũ���� ����
        if (isCrouch)
            Crouch();

        myRigid.linearVelocity = transform.up * jumpForce;
    }
    private void IsGround()
    {
        //ĸ���ݶ��̴� ������ y���� ���� �������� ���
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }


    //---------------- �÷��̾� ��ũ���� �޼ҵ� ------------------------
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

        //ī�޶��� Y��ġ�� ��ũ���� ������ŭ ����
        StartCoroutine(CrouchCoroutine());
    }

    //------------------------ ��ũ���� ��ȯ �ӵ� ���� --------------------------
    IEnumerator CrouchCoroutine()
    {
        //���� Y���� ���� - �⺻ Y������ �ʱ�ȭ
        //posY�� ��ũ���� Y���� �� ������ �ڷ�ƾ���� ����
        float posY = cam.transform.localPosition.y;
        int count = 0; 

        while(posY != applyCrouchPosY)
        {
            count++;
            //��ũ���� �ð� ����
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.4f);
            cam.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        //Lerp�� ����ϸ� ���� ��ǥ���� �� �������� �ʰ� ���� -> �ε巴�� �����Ű�� ���� ������� �ݺ� �� ���ϴ� ���� ��ǥ ������ ����
        cam.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
}
