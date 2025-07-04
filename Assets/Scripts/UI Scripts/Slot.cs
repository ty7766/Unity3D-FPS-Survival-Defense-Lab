using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    [Header("획득 아이템 속성")]
    public Item item;           //획득 아이템
    public int itemCount;       //획득 아이템 개수
    public Image itemImage;     //획득 아이템 이미지

    [Header("필요 컴포넌트")]
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private GameObject go_CountImage;

    //--------------------------- 인벤토리에 획득 아이템 넣기 -------------------------------
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;                                   //획득 아이템 넣기
        itemCount = _count;                             //획득 아이템 개수 넣기
        itemImage.sprite = item.itemImage;              //획득 아이템 이미지 삽입

        //획득 아이템이 장비면 개수 칸 활성화 X
        if(item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);                  //개수 칸 활성화
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            go_CountImage.SetActive(false);
        }

        SetColor(1);
    }

    //---------------------------- 아이템 칸 투명화 -----------------------------------
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    //------------------------- 획득 아이템 개수 조정 ------------------------
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        textCount.text = itemCount.ToString();

        //아이템이 없는 경우 슬롯 삭제
        if (itemCount <= 0)
            ClearSlot();
    }

    //------------------------ 슬롯 삭제 ----------------------------
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
