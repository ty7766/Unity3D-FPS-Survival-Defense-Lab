using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("무기 움직임 관련")]
    public Vector3 limitPos;                //무기의 최대 움직임 위치
    public Vector3 fineSightLimitPos;       //정조준 무기의 최대 움직임 위치
    public Vector3 smoothSway;              //움직임 정도

    [Header("연결 컴포넌트")]
    public GunController gunController;
    
    private Vector3 originPos;      //무기 기본 위치
    private Vector3 currentPos;     //무기 현재 위치

    //초기화
    void Start()
    {
        //무기의 기존 위치 생성
        originPos = this.transform.localPosition;
    }

    void Update()
    {
        if(GameManager.canPlayerMove)
            TrySway();
    }

    private void TrySway()
    {
        //마우스 움직였을 때
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        //마우스 움직이지 않았을 때
        else
            BackToOriginPos();
    }

    //-------------------------- 마우스 이동에 따른 무기 움직임 -----------------------
    private void Swaying()
    {
        //움직인 후의 위치
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        //총이 마우스를 살짝 뒤늦게 따라오는 것 구현
        //정조준X
        if (!gunController.isFineSightMode)
        {
            //마우스 속도가 너무 빨라 총이 뒤따라 못오는 현상 방지 -> Clamp 사용
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.x), -limitPos.y, limitPos.y),
                       originPos.z);
        }
        //정조준O
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.y), -fineSightLimitPos.x, fineSightLimitPos.x),
                       Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                       originPos.z);
        }

        transform.localPosition = currentPos;
    }

    //---------------------------- 마우스가 멈출 때 총의 원위치 복귀 -------------------------
    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
