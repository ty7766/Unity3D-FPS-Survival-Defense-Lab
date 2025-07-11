using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;          //�κ��丮 â�� ��� ���콺�� ī�޶� ȸ�� ��Ȱ��

    [Header("�ʿ� ������Ʈ")]
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;                      //GridSetting

    private Slot[] slots;
    
    void Start()
    {
        //�ڽ� ���Ե��� �Ӽ��� �Ѳ����� ����
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    //------------------------ �κ��丮 ����/�ݱ� --------------------------
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

    //----------------------------- ������ ȹ�� ------------------------------
    public void AcquireItem(Item _item, int _count = 1)
    {
        //ȹ�� �������� ��� �ƴ� ���
        if(Item.ItemType.Equipment != _item.itemType)
        {
            //�κ��丮�� ���� �������� �ִ� ��� ������ ���� �߰�
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

        //�κ��丮�� ���� �������� ���� ��� ���Կ� ������ �߰�
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
