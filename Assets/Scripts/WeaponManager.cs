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
    public Hand[] hands;

    [Header("���� ������Ʈ")]
    public GunController gunController;
    public HandController handController;

    //���� ���� ������ ���� ��ųʸ� ����
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDic = new Dictionary<string, Hand>();



    void Start()
    {
        for(int i =0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);           //Ű ������ ���� ���� �Է¹ް�, guns ������ ��ȯ
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].handName, hands[i]);       //Ű ������ ���� ���� �Է¹ް�, hands ������ ��ȯ
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
        }
    }

    //------------------- ���� ��ü -------------------------
    private void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
            gunController.GunChange(gunDic[_name]);
        else if (_type == "HAND")
            handController.HandChange(handDic[_name]);
    }
}
