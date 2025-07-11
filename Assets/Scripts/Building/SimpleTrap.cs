using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    [Header("트랩 설정")]
    [SerializeField]
    private GameObject go_Meat;             //트랩 고기
    [SerializeField]
    private int damage;                     //트랩 대미지

    [Header("연결 컴포넌트")]
    [SerializeField]
    private AudioClip sound_Activated;  //함정 사운드

    private bool isActivated = false;           //함정 가동 유무

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
            if (other.CompareTag("Player")) // 태그도 정확하게
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
                    Debug.LogWarning("씬에서 StatusController를 찾을 수 없습니다!");
                }
            }
        }
    }
}