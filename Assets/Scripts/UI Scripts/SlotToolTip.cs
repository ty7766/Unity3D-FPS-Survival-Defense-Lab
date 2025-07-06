using UnityEngine;
using UnityEngine.UI;


public class SlotToolTip : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    [SerializeField]
    private GameObject go_Base;
    [SerializeField]
    private Text textItemName;
    [SerializeField]
    private Text textItemDesc;
    [SerializeField]
    private Text textItemHow;

    //-------------------------- ������ ���� --------------------------
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);

        //���� ��ġ ����
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0f);
        go_Base.transform.position = _pos;

        //������ �̸� ����
        textItemName.text = _item.itemName;
        //������ ���� ����
        textItemDesc.text = _item.itemDesc;
        //������ ��� ����
        if (_item.itemType == Item.ItemType.Equipment)
            textItemHow.text = "��Ŭ�� - ����";
        else if (_item.itemType == Item.ItemType.Used)
            textItemHow.text = "��Ŭ�� - �Ա�";
        else
            textItemHow.text = "";
    }
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
