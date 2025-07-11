using UnityEditor.PackageManager.UI;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string carftName;                //이름
    public GameObject go_Prefab;            //설치될 오브젝트
    public GameObject go_PreviewPrefab;     //프리뷰 오브젝트
}

public class CraftManual : MonoBehaviour
{

    [Header("연결 컴포넌트")]
    [SerializeField]
    private GameObject go_BaseUI;           //기본 베이스 UI
    [SerializeField]
    private Craft[] craft_fire;             //모닥불용 탭
    [SerializeField]
    private Transform player;               //플레이어 위치
    [SerializeField]
    private float range;                    //프리팹 생성 거리
    [SerializeField]
    private LayerMask layerMask;


    private RaycastHit hitInfo;
    private GameObject go_Prefab;           //실제 생성될 프리팹
    private bool isActivated;               //UI 활성화/비활성화
    private bool isPreviewActivated;        //미리보기 활성화/비활성화
    private GameObject go_Preview;          //미리보기 프리팹


    void Update()
    {
        //TAB을 누르면 건설 창 열기
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            TabWindow();
        }

        //프리팹이 마우스를 따라가게 하기
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }

        //좌클릭하면 건설
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }

        //ESC를 누르면 건설 취소
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

    //--------------------------------- 탭 열기 ---------------------------------
    private void OpenWindow()
    {
        GameManager.isOpenCraftManual = true;
        isActivated = true;
        go_BaseUI.SetActive(true);
    }
    //--------------------------------- 탭 닫기 ---------------------------------
    private void CloseWindow()
    {
        GameManager.isOpenCraftManual = false;
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
    //--------------------------------- 건설 취소 ---------------------------------
    private void CancleBuild()
    {
        if(isPreviewActivated)
            Destroy(go_Preview);

        //건설 세팅 초기화
        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }


    //--------------------------------- 미리보기 프리팹 생성 ---------------------------------
    public void SlotClick(int _slotNumber)
    {
        //건설하고자 하는 오브젝트를 누르면 미리보기 프리팹 생성
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, player.position + player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        //탭 닫기
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    //--------------------------------- 프리팹이 마우스를 따라가게 하기 ------------------------
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

    //------------------------- 건설 메소드 ------------------------
    private void Build()
    {
        //건설 가능한 지형인지 확인
        if(isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);

            //초기화
            isActivated= false;
            isPreviewActivated= false;
            go_Preview= null;
            go_Prefab= null;
        }
    }

}
