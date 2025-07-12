using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;            //플레이어 움직임 제어
    public static bool isOpenInventory = false;         //인벤토리 활성화/비활성화
    public static bool isOpenCraftManual = false;       //건축 메뉴창 활성화/비활성화

    public static bool isNight = false;                 //밤인지 아닌지
    public static bool isWater = false;                 //물속인지 아닌지
    public static bool isPause = false;                 //일시정지 상태인지 아닌지

    private WeaponManager weaponManager;
    private bool flag = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;           //마우스 커서 고정
        Cursor.visible = false;
        weaponManager = FindAnyObjectByType<WeaponManager>();
    }

    void Update()
    {
        //마우스가 필요한 동작에 마우스 오픈
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        //이동 중일 때는 마우스 숨김
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        //물속에 있으면 무기 숨김
        if (isWater)
        {
            if (!flag)
            {
                StopAllCoroutines();
                StartCoroutine(weaponManager.WeaponInCoroutine());
                flag = true;
            }
        }
        else
        {
            if(flag)
            {
                weaponManager.WeaponOut();
                flag = false;
            }
        }
    }
}
