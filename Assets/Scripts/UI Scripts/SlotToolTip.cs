using UnityEngine;
using UnityEngine.UI;


public class SlotToolTip : MonoBehaviour
{
    [Header("연결 컴포넌트")]
    [SerializeField]
    private GameObject go_Base;
    [SerializeField]
    private Text textItemName;
    [SerializeField]
    private Text textItemDesc;
    [SerializeField]
    private Text textItemHow;

    //-------------------------- 아이템 툴팁 --------------------------
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);

        //툴팁 위치 적용
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0f);
        go_Base.transform.position = _pos;

        //아이템 이름 적용
        textItemName.text = _item.itemName;
        //아이템 설명 적용
        textItemDesc.text = _item.itemDesc;
        //아이템 사용 적용
        if (_item.itemType == Item.ItemType.Equipment)
            textItemHow.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            textItemHow.text = "우클릭 - 먹기";
        else
            textItemHow.text = "";
    }
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
