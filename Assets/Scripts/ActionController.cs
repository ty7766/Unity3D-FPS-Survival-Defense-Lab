using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField]
    private float range;            //���� ���� �ִ� �Ÿ�
    [SerializeField]
    private LayerMask layerMask;    //������ ���̾�� ��ȣ�ۿ� �ǵ��� ����
    [SerializeField]
    private Text actionText;        //���� ���ɽ� �ؽ�Ʈ

    private bool pickupActivated;   //���� ���ɽ� Ȱ��ȭ

    private RaycastHit hitInfo;     //�浹ü ����


    void Update()
    {
        TryAction();
        CheckItem();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        //�տ� �ִ� ������Ʈ�� ��ȣ�ۿ�
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            //������Ʈ�� Item�̸� ������ ���� ���
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    //-------------------------- ������ ȹ�� �ؽ�Ʈ Ȱ��/��Ȱ�� -----------------------------
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� " + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    //------------------------- ������ ȹ�� �޼ҵ� ---------------------------
    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ���߽��ϴ�!");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
}
