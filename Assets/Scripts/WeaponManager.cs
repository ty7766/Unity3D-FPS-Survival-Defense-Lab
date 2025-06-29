using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("무기 교체 관련")]
    //static으로 선언하여 현재 들고 있는 무기에서 또 현재 들고 있는 무기로 교체가 되지 않게끔 방지
    public static bool isChangeWeapon;                  //클래스 자체 변수
    public static Transform currentWeapon;              //현재 무기 활성화/비활성화
    public static Animator currentWeaponAnimator;       //현재 무기 애니메이션

    public float changeWeaponDelayTime;         //무기 교체 딜레이
    public float changeWeaponEndDelayTime;      //무기 교체가 끝난 시간
    public string currentWeaponType;            //현재 무기 타입

    [Header("무기 종류")]
    public Gun[] guns;
    public CloseWeapon[] hands;
    public CloseWeapon[] axes;
    public CloseWeapon[] pickaxes;

    [Header("연결 컴포넌트")]
    public GunController gunController;
    public HandController handController;
    public AxeController axeController;
    public PickaxeController pickaxeController;

    //무기 종류 관리를 위한 딕셔너리 생성
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDic = new Dictionary<string, CloseWeapon>();



    void Start()
    {
        for(int i =0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);           //키 값으로 무기 명을 입력받고, guns 정보를 반환
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].closeWeaponName, hands[i]);       //키 값으로 무기 명을 입력받고, hands 정보를 반환
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDic.Add(axes[i].closeWeaponName, axes[i]);       //키 값으로 무기 명을 입력받고, axes 정보를 반환
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDic.Add(pickaxes[i].closeWeaponName, pickaxes[i]);       //키 값으로 무기 명을 입력받고, axes 정보를 반환
        }
    }

    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
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

        //이전 무기의 행동들을 중지(ex.재장전, 조준 등)
        CancelPreWeaponAction();
        //이전 무기 -> 다음 무기 바꾸기
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        //바뀐 무기를 현재 무기로 설정
        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    //-------------------- 이전에 쓰던 무기 행동 중지 ------------------------
    public void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                gunController.CancelFineSight();        //조준 강제 취소
                gunController.CancelReload();           //재장전 강제 취소
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

    //------------------- 무기 교체 -------------------------
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
