using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    [Header("물속 속성")]
    [SerializeField]
    private float waterDrag;                //물 중력
    [SerializeField]
    private Color waterColor;               //물속 화면
    [SerializeField]
    private float waterFogDensity;          //물 탁함 정도

    [Header("물속 산소량 UI")]
    [SerializeField]
    private float totalOxygen;              //총 산소량
    private float currentOxygen;            //현재 산소량
    [SerializeField]
    private Text text_currentOxygen;
    [SerializeField]
    private Image image_gauge;
    [SerializeField]
    private GameObject go_BaseUI;
    [SerializeField]
    private Text text_totalOxygen;

    //원래 값
    private float originDrag;
    private Color originColor;
    private float originFogDensity;

    [Header("환경 속성")]
    [SerializeField]
    private Color originNightColor;
    [SerializeField]
    private float originNightFogDensity;
    [SerializeField]
    private Color waterNightColor;          //밤 물속 색
    [SerializeField]
    private float waterNightFogDensity;
    [SerializeField]
    private float breatheTime;

    private float currentBreatheTime;
    private float temp;                     //산소량 감소 속도 관리 위함

    [Header("사운드")]
    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_Breathe;
    
    private StatusController statusController;
    
    //초기화
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

        //산소량 감소
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

    //-------------------------------------- 물 속에 들어감 ---------------------------------------
    private void GetWater(Collider _player)
    {
        //사운드 재생
        SoundManager.instance.PlaySoundEffects(sound_WaterIn);

        //UI 활성
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

    //-------------------------------------- 물 속에서 나옴 ---------------------------------------
    private void GetOutWater(Collider _player)
    {
        if(GameManager.isWater)
        {
            //UI 비활성
            go_BaseUI.SetActive(false);
            //산소 초기화
            currentOxygen = totalOxygen;
            //사운드 재생
            SoundManager.instance.PlaySoundEffects(sound_WaterOut);

            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().linearDamping = originDrag;

            //밤일때 물속에서 나올 때 Fog가 사라지는 것 방지
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

    //------------------------- 산소량 감소 ------------------------------
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
