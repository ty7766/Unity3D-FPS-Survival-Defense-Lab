using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Header("�� �Ӽ�")]
    [SerializeField]
    private float secondPerRealTimeSecond;              //���� 100�� = ���� 1��
    [SerializeField]
    private float nightFogDensity;                      //�� ������ Fog �е�
    [SerializeField]
    private float fogDensityCalc;                       //������ ����


    private float dayFogDensity;                        //�� ������ Fog �е�
    private float currentFogDensity;                    //���


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
