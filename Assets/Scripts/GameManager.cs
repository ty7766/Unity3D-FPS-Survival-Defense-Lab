using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;            //�÷��̾� ������ ����
    public static bool isOpenInventory = false;         //�κ��丮 Ȱ��ȭ/��Ȱ��ȭ
    public static bool isOpenCraftManual = false;       //���� �޴�â Ȱ��ȭ/��Ȱ��ȭ

    public static bool isNight = false;                 //������ �ƴ���
    public static bool isWater = false;                 //�������� �ƴ���

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;           //���콺 Ŀ�� ����
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
