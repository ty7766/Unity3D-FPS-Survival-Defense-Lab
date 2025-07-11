using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    [Header("Ʈ�� ����")]
    [SerializeField]
    private GameObject go_Meat;             //Ʈ�� ���
    [SerializeField]
    private int damage;                     //Ʈ�� �����

    [Header("���� ������Ʈ")]
    [SerializeField]
    private AudioClip sound_Activated;  //���� ����

    private bool isActivated = false;           //���� ���� ����

    private AudioSource audioSource;
    private Rigidbody[] rd;

    void Start()
    {
        rd = GetComponentsInChildren<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            if (other.CompareTag("Player")) // �±׵� ��Ȯ�ϰ�
            {
                isActivated = true;
                audioSource.clip = sound_Activated;
                audioSource.Play();

                Destroy(go_Meat);

                for (int i = 0; i < rd.Length; i++)
                {
                    rd[i].isKinematic = false;
                    rd[i].useGravity = true;
                }

                StatusController statusController = FindAnyObjectByType<StatusController>();
                if (statusController != null)
                {
                    statusController.DecreaseHP(damage);
                }
                else
                {
                    Debug.LogWarning("������ StatusController�� ã�� �� �����ϴ�!");
                }
            }
        }
    }
}