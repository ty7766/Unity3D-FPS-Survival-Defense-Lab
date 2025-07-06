using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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

    private ItemEffectDatabase itemEffectDatabase;

    void Start()
    {
        itemEffectDatabase = FindAnyObjectByType<ItemEffectDatabase>();
    }

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

    //--------------------------- 슬롯에 있는 아이템 상호작용 ---------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        //오브젝트에 우클릭하면 실행
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                //재료인 경우 소모
                itemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1);
            }
        }
    }

    //-------------------------------- 드래그 -------------------------------
    //드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        //아이템이 있을때만 드래그 가능
        if(item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);

            DragSlot.instance.transform.position = eventData.position;

        }
    }
    //드래그 중
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }
    //드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;

    }
    //다른 슬롯에 아이템 드랍 시
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
        {
            //슬롯에서 드래그가 끝났을 때만 스왑하도록 설정
            ChangeSlot();
        }
    }

    //--------------------------- 인벤토리 스왑 -----------------------------
    //1. 아이템 A를 B에 드랍
    //2. B의 복사본 생성
    //3. 기존 B를 삭제 및 A 삽입
    //4. 기존 A 위치에 B의 복사본 삽입
    private void ChangeSlot()
    {
        //2
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        //1
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //3
        //바뀔 위치에 아이템이 이미 있는 경우 
        if ( _tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        //바뀔 위치에 아이템이 없는 경우
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}
