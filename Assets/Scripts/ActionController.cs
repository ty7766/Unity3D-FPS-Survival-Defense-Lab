using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [Header("습득 정보 관련")]
    [SerializeField]
    private float range;            //습득 가능 최대 거리
    [SerializeField]
    private LayerMask layerMask;    //아이템 레이어에만 상호작용 되도록 설정
    [SerializeField]
    private Text actionText;        //습득 가능시 텍스트

    private bool pickupActivated;   //습득 가능시 활성화

    private RaycastHit hitInfo;     //충돌체 정보


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
        //앞에 있는 오브젝트와 상호작용
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            //오브젝트가 Item이면 아이템 정보 출력
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    //-------------------------- 아이템 획득 텍스트 활성/비활성 -----------------------------
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    //------------------------- 아이템 획득 메소드 ---------------------------
    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다!");
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
}
