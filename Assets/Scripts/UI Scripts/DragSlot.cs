using UnityEngine;
using UnityEngine.UI;


//--------------------- ������ �巡�� �� �� �̹����� �����Ͽ� ���� ---------------------------------
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public Slot dragSlot;

    [Header("������ �̹���")]
    [SerializeField]
    private Image imageItem;

    private void Start()
    {
        instance = this;
    }
    
    //�巡�� �� �̹��� ����
    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    //���� ����
    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
