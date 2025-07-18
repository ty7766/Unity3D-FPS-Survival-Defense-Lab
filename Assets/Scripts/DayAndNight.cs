using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Header("해 속성")]
    [SerializeField]
    private float secondPerRealTimeSecond;              //게임 100초 = 현실 1초
    [SerializeField]
    private float nightFogDensity;                      //밤 상태의 Fog 밀도
    [SerializeField]
    private float fogDensityCalc;                       //증감량 비율


    private float dayFogDensity;                        //낮 상태의 Fog 밀도
    private float currentFogDensity;                    //계산


    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            GameManager.isNight = true;
        else if (transform.eulerAngles.x >= 340)
            GameManager.isNight = false;

        if (GameManager.isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if(currentFogDensity >= dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
