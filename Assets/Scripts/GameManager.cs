using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool canPlayerMove = true;            //�÷��̾� ������ ����
    public static bool isOpenInventory = false;         //�κ��丮 Ȱ��ȭ/��Ȱ��ȭ
    public static bool isOpenCraftManual = false;       //���� �޴�â Ȱ��ȭ/��Ȱ��ȭ

    public static bool isNight = false;                 //������ �ƴ���
    public static bool isWater = false;                 //�������� �ƴ���
    public static bool isPause = false;                 //�Ͻ����� �������� �ƴ���

    private WeaponManager weaponManager;
    private bool flag = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;           //���콺 Ŀ�� ����
        Cursor.visible = false;
        weaponManager = FindAnyObjectByType<WeaponManager>();
    }

    void Update()
    {
        //���콺�� �ʿ��� ���ۿ� ���콺 ����
        if (isOpenInventory || isOpenCraftManual || isPause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        //�̵� ���� ���� ���콺 ����
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }

        //���ӿ� ������ ���� ����
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
