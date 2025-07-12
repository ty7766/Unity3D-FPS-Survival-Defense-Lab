using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("���� ������ ����")]
    public Vector3 limitPos;                //������ �ִ� ������ ��ġ
    public Vector3 fineSightLimitPos;       //������ ������ �ִ� ������ ��ġ
    public Vector3 smoothSway;              //������ ����

    [Header("���� ������Ʈ")]
    public GunController gunController;
    
    private Vector3 originPos;      //���� �⺻ ��ġ
    private Vector3 currentPos;     //���� ���� ��ġ

    //�ʱ�ȭ
    void Start()
    {
        //������ ���� ��ġ ����
        originPos = this.transform.localPosition;
    }

    void Update()
    {
        if(GameManager.canPlayerMove)
            TrySway();
    }

    private void TrySway()
    {
        //���콺 �������� ��
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        //���콺 �������� �ʾ��� ��
        else
            BackToOriginPos();
    }

    //-------------------------- ���콺 �̵��� ���� ���� ������ -----------------------
    private void Swaying()
    {
        //������ ���� ��ġ
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        //���� ���콺�� ��¦ �ڴʰ� ������� �� ����
        //������X
        if (!gunController.isFineSightMode)
        {
            //���콺 �ӵ��� �ʹ� ���� ���� �ڵ��� ������ ���� ���� -> Clamp ���
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                       originPos.z);
        }
        //������O
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                       originPos.z);
        }

        transform.localPosition = currentPos;
    }

    //---------------------------- ���콺�� ���� �� ���� ����ġ ���� -------------------------
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
