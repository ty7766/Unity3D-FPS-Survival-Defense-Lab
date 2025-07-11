using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;            //플레이어 움직임 제어
    public static bool isOpenInventory = false;         //인벤토리 활성화/비활성화
    public static bool isOpenCraftManual = false;       //건축 메뉴창 활성화/비활성화

    public static bool isNight = false;                 //밤인지 아닌지
    public static bool isWater = false;                 //물속인지 아닌지

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;           //마우스 커서 고정
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpenInventory || isOpenCraftManual)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

    }
}
