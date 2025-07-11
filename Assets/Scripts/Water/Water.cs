using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("물속 속성")]
    [SerializeField]
    private float waterDrag;                //물 중력
    [SerializeField]
    private Color waterColor;               //물속 화면
    [SerializeField]
    private float waterFogDensity;          //물 탁함 정도

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

    [Header("사운드")]
    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_Breathe;

    
    //초기화
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

    //-------------------------------------- 물 속에 들어감 ---------------------------------------
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

    //-------------------------------------- 물 속에서 나옴 ---------------------------------------
    private void GetOutWater(Collider _player)
    {
        if(GameManager.isWater)
        {
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
}
