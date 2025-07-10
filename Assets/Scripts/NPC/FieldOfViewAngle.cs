using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [Header("�þ� ����")]
    [SerializeField]
    private float viewAngle;        //�þ߰�
    [SerializeField]
    private float viewDistance;     //�þ� �Ÿ�
    [SerializeField]
    private LayerMask targetMask;   //�ش� Ÿ��

    private Pig pig;                //pig ������Ʈ

    private void Start()
    {
        pig = GetComponent<Pig>();
    }
    private void Update()
    {
        View();
    }

    //------------------------------- �þ߰� ���� ---------------------------------
    private void View()
    {
        //z���� �������� ���� ���ݸ�ŭ�� ���ʿ� ���� �ο�
        Vector3 leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, rightBoundary, Color.red);

        //�÷��̾� Mask�� ���̴��� �����Ͽ� �÷��̾ �þ߰��� ������ ��ȣ�ۿ�ǰ� ����
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i = 0; i < _target.Length; i++)
        {
           Transform _targetTf = _target[i].transform;

            if(_targetTf.name == "Player")
            {
                //1. �÷��̾� - ������Ʈ �� ���� ���ϱ�
                //2. (1)�� ���Ϳ� ������Ʈ z�� ���� ���̰� ���ϱ�
                Vector3 _direc = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direc, transform.forward);

                //3. �� ���̰��� ��ü �þ߰��� ���ݺ��� ������ �þ߰��� ���� ��
                if (_angle < viewAngle * 0.5f)
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position + transform.up, _direc, out hitInfo, viewDistance))
                    {
                        //4. �÷��̾� - ������Ʈ �� ��ֹ��� �ִ� ���
                        if (hitInfo.transform.name == "Player")
                        {
                            Debug.Log("�÷��̾ ���� �þ� ���� �ֽ��ϴ�");
                            Debug.DrawRay(transform.position + transform.up, _direc, Color.blue);
                            //5. �÷��̾��� �ݴ�������� �ٱ�
                            pig.Run(hitInfo.transform.position);
                        }
                    }
                }
            }
        }
    }
    private Vector3 BoundaryAngle(float _angle)
    {
        //y���� �������� z���� ȸ���ϱ� ������
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }
}
