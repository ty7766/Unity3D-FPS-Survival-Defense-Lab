using UnityEngine;
using UnityEngine.UI;


//--------------------- 아이템 드래그 시 새 이미지를 생성하여 복사 ---------------------------------
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;

    [Header("아이템 이미지")]
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this;
    }
    
    //드래그 시 이미지 생성
    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    //투명도 관리
    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
