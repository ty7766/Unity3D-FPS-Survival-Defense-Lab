using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]
    private int hp;
    private int currentHp;

    [Header("SP")]
    [SerializeField]
    private int sp;
    private int currentSp;
    [SerializeField]
    private int spIncreaseSpeed;            //SP ���� �ӵ�
    [SerializeField]
    private int spRechargeTime;             //SP ȸ�� ������
    private int currentSpRechargeTime;

    private bool spUsed;

    [Header("DP")]
    [SerializeField]
    private int dp;
    private int currentDp;

    [Header("HUNGRY")]
    [SerializeField]
    private int hungry;
    private int currentHungry;
    [SerializeField]
    private int hungryDecreaseTime;                     //����� ���� �ð�
    private int currentHungryDecreaseTime;

    [Header("THIRSTY")]
    [SerializeField]
    private int thirsty;
    private int currentThirsty;
    [SerializeField]
    private int thirstyDecreaseTime;                    //�񸶸� ���� �ð�
    private int currentThirstyDecreaseTime;

    [Header("SATISFY")]
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    [Header("���� ������Ʈ")]
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;         //Status ��ȣ �ο�


    //�ʱ�ȭ
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        SPRecover();
        GaugeUpdate();
    }

    //---------------------------- ����� ��ġ ----------------------------
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            //����� ���� �ð��� ������ �ð��� �����ϸ� ����� ��ġ�� 1���� ���ҽ�Ű��
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("����� ��ġ�� 0�� �Ǿ����ϴ�!");
    }

    //------------------------------ ���� ��ġ -----------------------------
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            //����� ���� �ð��� ������ �ð��� �����ϸ� ����� ��ġ�� 1���� ���ҽ�Ű��
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("���� ��ġ�� 0�� �Ǿ����ϴ�!");
    }

    //------------------------ UI ������ ���� -----------------------------
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    //------------------------- SP ��ġ --------------------------------
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }
    //SP ȸ�� ��� �ð�
    private void SPRechargeTime()
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }
    //SP ȸ��
    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }
}
