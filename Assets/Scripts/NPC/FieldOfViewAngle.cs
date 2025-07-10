using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [Header("시야 설정")]
    [SerializeField]
    private float viewAngle;        //시야각
    [SerializeField]
    private float viewDistance;     //시야 거리
    [SerializeField]
    private LayerMask targetMask;   //해당 타겟

    private Pig pig;                //pig 컴포넌트

    private void Start()
    {
        pig = GetComponent<Pig>();
    }
    private void Update()
    {
        View();
    }

    //------------------------------- 시야각 적용 ---------------------------------
    private void View()
    {
        //z축을 기준으로 각각 절반만큼씩 양쪽에 각도 부여
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        //플레이어 Mask만 레이더에 적용하여 플레이어가 시야각에 들어오면 상호작용되게 설정
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i = 0; i < _target.Length; i++)
        {
           Transform _targetTf = _target[i].transform;

            if(_targetTf.name == "Player")
            {
                //1. 플레이어 - 오브젝트 간 벡터 구하기
                //2. (1)의 벡터와 오브젝트 z축 벡터 사이각 구하기
                Vector3 _direc = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direc, transform.forward);

                //3. 이 사이각이 전체 시야각의 절반보다 작으면 시야각에 들어온 것
                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position + transform.up, _direc, out hitInfo, viewDistance))
                    {
                        //4. 플레이어 - 오브젝트 간 장애물이 있는 경우
                        if (hitInfo.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다");
                            Debug.DrawRay(transform.position + transform.up, _direc, Color.blue);
                            //5. 플레이어의 반대방향으로 뛰기
                            pig.Run(hitInfo.transform.position);
                        }
                    }
                }
            }
        }
    }
    private Vector3 BoundaryAngle(float _angle)
    {
        //y축을 기준으로 z축이 회전하기 때문에
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }
}
