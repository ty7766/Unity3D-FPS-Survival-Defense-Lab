using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [Header("���� �Ӽ�")]
    [SerializeField]
    private float waterDrag;                //�� �߷�
    [SerializeField]
    private Color waterColor;               //���� ȭ��
    [SerializeField]
    private float waterFogDensity;          //�� Ź�� ����

    [Header("���� ��ҷ� UI")]
    [SerializeField]
    private float totalOxygen;              //�� ��ҷ�
    private float currentOxygen;            //���� ��ҷ�
    [SerializeField]
    private Text text_currentOxygen;
    [SerializeField]
    private Image image_gauge;
    [SerializeField]
    private GameObject go_BaseUI;
    [SerializeField]
    private Text text_totalOxygen;

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
    private float temp;                     //��ҷ� ���� �ӵ� ���� ����

    [Header("����")]
    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_Breathe;
    
    private StatusController statusController;
    
    //�ʱ�ȭ
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;
        originDrag = 0;
        RenderSettings.fog = true;

        statusController = FindAnyObjectByType<StatusController>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
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

        //��ҷ� ����
        DecreaseOxygen();
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
        //���� ���
        SoundManager.instance.PlaySoundEffects(sound_WaterIn);

        //UI Ȱ��
        go_BaseUI.SetActive(true);

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
            //UI ��Ȱ��
            go_BaseUI.SetActive(false);
            //��� �ʱ�ȭ
            currentOxygen = totalOxygen;
            //���� ���
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

    //------------------------- ��ҷ� ���� ------------------------------
    private void DecreaseOxygen()
    {
        if(GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.RoundToInt(currentOxygen).ToString();
            image_gauge.fillAmount = currentOxygen / totalOxygen;

            if(currentOxygen <= 0)
            {
                temp += Time.deltaTime;
                if (temp >= 1)
                {
                    statusController.DecreaseDP(1);
                    temp = 0;
                }
            }
        }
    }
}
