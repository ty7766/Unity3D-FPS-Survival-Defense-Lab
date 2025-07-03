using UnityEngine;

public class Rock : MonoBehaviour
{

    [Header("���� �Ӽ�")]
    [SerializeField]
    private int hp;                 //������ ü��
    [SerializeField]
    private float destroyTime;      //���� ������ �������� �ð�
    [SerializeField]
    private int itemDropCount;      //������ ��� ����

    [Header("���� ������Ʈ")]
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private GameObject go_rock;                     //�Ϲ� ���� ������Ʈ
    [SerializeField]
    private GameObject go_debris;                   //���� ���� ������Ʈ
    [SerializeField]
    private GameObject go_effect_prefab;            //ä�� ����Ʈ
    [SerializeField]
    private GameObject go_rock_item_prefab;         //����� ������

    [Header("�ʿ� ����")]
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        //ä�� ����
        SoundManager.instance.PlaySoundEffects(strike_Sound);
        //ä�� ����Ʈ ����
        var clone = Instantiate(go_effect_prefab, sphereCollider.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime / 3);

        hp--;
        if (hp <= 0)
            Destruction();
    }

    //----------------------- ���� �ı� -------------------------
    private void Destruction()
    {
        //�ı� ����
        SoundManager.instance.PlaySoundEffects(destroy_Sound);

        //�Ϲ� ���� ���� -> ���� ���� Ȱ�� -> ���� ���� ����
        sphereCollider.enabled = false;
        
        //������ ���
        for (int i = 0; i < itemDropCount; i++)
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position, Quaternion.identity);
        }

        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
