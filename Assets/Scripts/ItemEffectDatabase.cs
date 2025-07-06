using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemEffect
{
    public string itemName;         //아이템 명
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다")]
    public string[] part;           //적용 부위
    public int[] num;               //수치

}
public class ItemEffectDatabase : MonoBehaviour
{
    [Header("아이템 효과")]
    [SerializeField]
    private ItemEffect[] itemEffects;

    [Header("연결 컴포넌트")]
    [SerializeField]
    private StatusController statusController;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private SlotToolTip slotToolTip;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    //-------------------------- 아이템 사용 -----------------------------
    public void UseItem(Item _item)
    {
        //장비인 경우 장착
        if (_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(weaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }

        else if (_item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName)
                {
                    for (int j = 0; j < itemEffects[i].part.Length;  j++)
                    {
                        switch(itemEffects[i].part[j])
                        {
                            case HP:
                                statusController.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case SP:
                                statusController.IncreaseSP(itemEffects[i].num[j]);
                                break;
                            case DP:
                                statusController.IncreaseDP(itemEffects[i].num[j]);
                                break;
                            case HUNGRY:
                                statusController.IncreaseHUNGRY(itemEffects[i].num[j]);
                                break;
                            case THIRSTY:
                                statusController.IncreaseTHIRSTY(itemEffects[i].num[j]);
                                break;
                            case SATISFY:
                                statusController.IncreaseSATISFY(itemEffects[i].num[j]);
                                break;
                            default:
                                Debug.Log("잘못된 Status 부위! HP, SP, DP, HUNGRY, THIRSTY, SATISFY만 가능합니다");
                                break;
                        }
                        Debug.Log(_item.itemName + " 을 사용했습니다!");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase에 일치하는 itemName이 없습니다");
        }
    }
    
    //--------------------------- 슬롯 툴팁 활성 ----------------------------
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        slotToolTip.ShowToolTip(_item, _pos);
    }
    //--------------------------- 슬롯 툴팁 비활성 ----------------------------
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }
}
