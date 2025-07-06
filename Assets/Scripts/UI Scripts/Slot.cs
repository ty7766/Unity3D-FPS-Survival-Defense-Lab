using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("ȹ�� ������ �Ӽ�")]
    public Item item;           //ȹ�� ������
    public int itemCount;       //ȹ�� ������ ����
    public Image itemImage;     //ȹ�� ������ �̹���

    [Header("�ʿ� ������Ʈ")]
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private GameObject go_CountImage;

    private ItemEffectDatabase itemEffectDatabase;

    void Start()
    {
        itemEffectDatabase = FindAnyObjectByType<ItemEffectDatabase>();
    }

    //--------------------------- �κ��丮�� ȹ�� ������ �ֱ� -------------------------------
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;                                   //ȹ�� ������ �ֱ�
        itemCount = _count;                             //ȹ�� ������ ���� �ֱ�
        itemImage.sprite = item.itemImage;              //ȹ�� ������ �̹��� ����

        //ȹ�� �������� ���� ���� ĭ Ȱ��ȭ X
        if(item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);                  //���� ĭ Ȱ��ȭ
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    //---------------------------- ������ ĭ ����ȭ -----------------------------------
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //------------------------- ȹ�� ������ ���� ���� ------------------------
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

        //�������� ���� ��� ���� ����
        if (itemCount <= 0)
            ClearSlot();
    }

    //------------------------ ���� ���� ----------------------------
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        textCount.text = "0";
        go_CountImage.SetActive(false);
    }

    //--------------------------- ���Կ� �ִ� ������ ��ȣ�ۿ� ---------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        //������Ʈ�� ��Ŭ���ϸ� ����
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                //����� ��� �Ҹ�
                itemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }

    //-------------------------------- �巡�� -------------------------------
    //�巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�������� �������� �巡�� ����
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;

        }
    }
    //�巡�� ��
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }
    //�巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;

    }
    //�ٸ� ���Կ� ������ ��� ��
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            //���Կ��� �巡�װ� ������ ���� �����ϵ��� ����
            ChangeSlot();
        }
    }

    //--------------------------- �κ��丮 ���� -----------------------------
    //1. ������ A�� B�� ���
    //2. B�� ���纻 ����
    //3. ���� B�� ���� �� A ����
    //4. ���� A ��ġ�� B�� ���纻 ����
    private void ChangeSlot()
    {
        //2
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        //1
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //3
        //�ٲ� ��ġ�� �������� �̹� �ִ� ��� 
        if ( _tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        //�ٲ� ��ġ�� �������� ���� ���
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
