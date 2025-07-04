using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
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
}
