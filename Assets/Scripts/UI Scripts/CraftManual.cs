using UnityEditor.PackageManager.UI;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string carftName;                //�̸�
    public GameObject go_Prefab;            //��ġ�� ������Ʈ
    public GameObject go_PreviewPrefab;     //������ ������Ʈ
}

public class CraftManual : MonoBehaviour
{

    [Header("���� ������Ʈ")]
    [SerializeField]
    private GameObject go_BaseUI;           //�⺻ ���̽� UI
    [SerializeField]
    private Craft[] craft_fire;             //��ںҿ� ��
    [SerializeField]
    private Transform player;               //�÷��̾� ��ġ
    [SerializeField]
    private float range;                    //������ ���� �Ÿ�
    [SerializeField]
    private LayerMask layerMask;


    private RaycastHit hitInfo;
    private GameObject go_Prefab;           //���� ������ ������
    private bool isActivated;               //UI Ȱ��ȭ/��Ȱ��ȭ
    private bool isPreviewActivated;        //�̸����� Ȱ��ȭ/��Ȱ��ȭ
    private GameObject go_Preview;          //�̸����� ������


    void Update()
    {
        //TAB�� ������ �Ǽ� â ����
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            TabWindow();
        }

        //�������� ���콺�� ���󰡰� �ϱ�
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        //��Ŭ���ϸ� �Ǽ�
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        //ESC�� ������ �Ǽ� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancleBuild();
        }
    }

    private void TabWindow()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    //--------------------------------- �� ���� ---------------------------------
    private void OpenWindow()
    {
        GameManager.isOpenCraftManual = true;
        isActivated = true;
        go_BaseUI.SetActive(true);
    }
    //--------------------------------- �� �ݱ� ---------------------------------
    private void CloseWindow()
    {
        GameManager.isOpenCraftManual = false;
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
    //--------------------------------- �Ǽ� ��� ---------------------------------
    private void CancleBuild()
    {
        if(isPreviewActivated)
            Destroy(go_Preview);

        //�Ǽ� ���� �ʱ�ȭ
        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }


    //--------------------------------- �̸����� ������ ���� ---------------------------------
    public void SlotClick(int _slotNumber)
    {
        //�Ǽ��ϰ��� �ϴ� ������Ʈ�� ������ �̸����� ������ ����
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, player.position + player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        //�� �ݱ�
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    //--------------------------------- �������� ���콺�� ���󰡰� �ϱ� ------------------------
    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(player.position, player.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    //------------------------- �Ǽ� �޼ҵ� ------------------------
    private void Build()
    {
        //�Ǽ� ������ �������� Ȯ��
        if(isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);

            //�ʱ�ȭ
            isActivated= false;
            isPreviewActivated= false;
            go_Preview= null;
            go_Prefab= null;
        }
    }

}
