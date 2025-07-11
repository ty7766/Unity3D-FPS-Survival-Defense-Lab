using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("���� �Ӽ�")]
    [SerializeField]
    private float waterDrag;                //�� �߷�
    [SerializeField]
    private Color waterColor;               //���� ȭ��
    [SerializeField]
    private float waterFogDensity;          //�� Ź�� ����

    //���� ��
    private float originDrag;
    private Color originColor;
    private float originFogDensity;

    [Header("ȯ�� �Ӽ�")]
    [SerializeField]
    private Color originNightColor;
    [SerializeField]
    private float originNightFogDensity;
    [SerializeField]
    private Color waterNightColor;          //�� ���� ��
    [SerializeField]
    private float waterNightFogDensity;
    [SerializeField]
    private float breatheTime;

    private float currentBreatheTime;

    [Header("����")]
    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_Breathe;

    
    //�ʱ�ȭ
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;
        originDrag = 0;
        RenderSettings.fog = true;
    }

    private void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if(currentBreatheTime >= breatheTime)
            {
                SoundManager.instance.PlaySoundEffects(sound_Breathe);
                currentBreatheTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    //-------------------------------------- �� �ӿ� �� ---------------------------------------
    private void GetWater(Collider _player)
    {
        SoundManager.instance.PlaySoundEffects(sound_WaterIn);

        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().linearDamping = waterDrag;

        if(!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    //-------------------------------------- �� �ӿ��� ���� ---------------------------------------
    private void GetOutWater(Collider _player)
    {
        if(GameManager.isWater)
        {
            SoundManager.instance.PlaySoundEffects(sound_WaterOut);

            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().linearDamping = originDrag;

            //���϶� ���ӿ��� ���� �� Fog�� ������� �� ����
            if(!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
