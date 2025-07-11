using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;          //인벤토리 창인 경우 마우스의 카메라 회전 비활성

    [Header("필요 컴포넌트")]
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;                      //GridSetting

    private Slot[] slots;
    
    void Start()
    {
        //자식 슬롯들의 속성을 한꺼번에 관리
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    //------------------------ 인벤토리 열기/닫기 --------------------------
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }
    private void OpenInventory()
    {
        GameManager.isOpenInventory = true;
        go_InventoryBase.SetActive(true);
    }
    private void CloseInventory()
    {
        GameManager.isOpenInventory = false;
        go_InventoryBase.SetActive(false);
    }

    //----------------------------- 아이템 획득 ------------------------------
    public void AcquireItem(Item _item, int _count = 1)
    {
        //획득 아이템이 장비가 아닌 경우
        if(Item.ItemType.Equipment != _item.itemType)
        {
            //인벤토리에 기존 아이템이 있는 경우 아이템 개수 추가
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        //인벤토리에 기존 아이템이 없는 경우 슬롯에 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
