using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("���� ��ü ����")]
    //static���� �����Ͽ� ���� ��� �ִ� ���⿡�� �� ���� ��� �ִ� ����� ��ü�� ���� �ʰԲ� ����
    public static bool isChangeWeapon;                  //Ŭ���� ��ü ����
    public static Transform currentWeapon;              //���� ���� Ȱ��ȭ/��Ȱ��ȭ
    public static Animator currentWeaponAnimator;       //���� ���� �ִϸ��̼�

    public float changeWeaponDelayTime;         //���� ��ü ������
    public float changeWeaponEndDelayTime;      //���� ��ü�� ���� �ð�
    public string currentWeaponType;            //���� ���� Ÿ��

    [Header("���� ����")]
    public Gun[] guns;
    public CloseWeapon[] hands;
    public CloseWeapon[] axes;
    public CloseWeapon[] pickaxes;

    [Header("���� ������Ʈ")]
    public GunController gunController;
    public HandController handController;
    public AxeController axeController;
    public PickaxeController pickaxeController;

    //���� ���� ������ ���� ��ųʸ� ����
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDic = new Dictionary<string, CloseWeapon>();



    void Start()
    {
        for(int i =0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);           //Ű ������ ���� ���� �Է¹ް�, guns ������ ��ȯ
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].closeWeaponName, hands[i]);       //Ű ������ ���� ���� �Է¹ް�, hands ������ ��ȯ
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDic.Add(axes[i].closeWeaponName, axes[i]);       //Ű ������ ���� ���� �Է¹ް�, axes ������ ��ȯ
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDic.Add(pickaxes[i].closeWeaponName, pickaxes[i]);       //Ű ������ ���� ���� �Է¹ް�, axes ������ ��ȯ
        }
    }

    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnimator.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        //���� ������ �ൿ���� ����(ex.������, ���� ��)
        CancelPreWeaponAction();
        //���� ���� -> ���� ���� �ٲٱ�
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        //�ٲ� ���⸦ ���� ����� ����
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    //-------------------- ������ ���� ���� �ൿ ���� ------------------------
    public void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                gunController.CancelFineSight();        //���� ���� ���
                gunController.CancelReload();           //������ ���� ���
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;
        }
    }

    //------------------- ���� ��ü -------------------------
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
            gunController.GunChange(gunDic[_name]);
        else if (_type == "HAND")
            handController.CloseWeaponChange(handDic[_name]);
        else if (_type == "AXE")
            axeController.CloseWeaponChange(axeDic[_name]);
        else if (_type == "PICKAXE")
            pickaxeController.CloseWeaponChange(pickaxeDic[_name]);
    }
}
