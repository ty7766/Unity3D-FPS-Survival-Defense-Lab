using UnityEngine;

public class Rock : MonoBehaviour
{

    [Header("���� �Ӽ�")]
    [SerializeField]
    private int hp;                 //������ ü��
    [SerializeField]
    private float destroyTime;      //���� ������ �������� �ð�

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
    private AudioSource audioSource;                //ä�� ����Ʈ ����
    [SerializeField]
    private AudioClip effectSound;                  //ä�� ����
    [SerializeField]
    private AudioClip effectSound2;                 //�ı� ����

    public void Mining()
    {
        //ä�� ����
        audioSource.clip = effectSound;
        audioSource.Play();
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
        audioSource.clip = effectSound2;
        audioSource.Play();

        //�Ϲ� ���� ���� -> ���� ���� Ȱ�� -> ���� ���� ����
        sphereCollider.enabled = false;
        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
