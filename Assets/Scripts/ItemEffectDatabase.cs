using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemEffect
{
    public string itemName;         //������ ��
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�")]
    public string[] part;           //���� ����
    public int[] num;               //��ġ

}
public class ItemEffectDatabase : MonoBehaviour
{
    [Header("������ ȿ��")]
    [SerializeField]
    private ItemEffect[] itemEffects;

    [Header("���� ������Ʈ")]
    [SerializeField]
    private StatusController statusController;
    [SerializeField]
    private WeaponManager weaponManager;
    [SerializeField]
    private SlotToolTip slotToolTip;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    //-------------------------- ������ ��� -----------------------------
    public void UseItem(Item _item)
    {
        //����� ��� ����
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
                                Debug.Log("�߸��� Status ����! HP, SP, DP, HUNGRY, THIRSTY, SATISFY�� �����մϴ�");
                                break;
                        }
                        Debug.Log(_item.itemName + " �� ����߽��ϴ�!");
                    }
                    return;
                }
            }
            Debug.Log("ItemEffectDatabase�� ��ġ�ϴ� itemName�� �����ϴ�");
        }
    }
    
    //--------------------------- ���� ���� Ȱ�� ----------------------------
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        slotToolTip.ShowToolTip(_item, _pos);
    }
    //--------------------------- ���� ���� ��Ȱ�� ----------------------------
    public void HideToolTip()
    {
        slotToolTip.HideToolTip();
    }
}
