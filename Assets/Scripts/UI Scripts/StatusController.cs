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

    //------------------- HP ���� ---------------------
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    //------------------- HP ���� ---------------------
    public void DecreaseHP(int _count)
    {
        //DP�� �ִ� ��� DP ���� ����
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("ĳ������ hp�� 0�� �Ǿ����ϴ�!");
    }


    //------------------- SP ���� ---------------------
    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    //------------------- SP ���� ---------------------
    public void DecreaseSP(int _count)
    {
        currentSp -= _count;

        if (currentSp <= 0)
            Debug.Log("ĳ������ sp�� 0�� �Ǿ����ϴ�!");
    }


    //------------------- DP ���� ---------------------
    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    //------------------- DP ���� ---------------------
    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("ĳ������ dp�� 0�� �Ǿ����ϴ�!");
    }


    //------------------- HUNGRY ���� ---------------------
    public void IncreaseHUNGRY(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    //------------------- HUNGRY ���� ---------------------
    public void DecreaseHUNGRY(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }


    //------------------- THIRSTY ���� ---------------------
    public void IncreaseTHIRSTY(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    //------------------- THIRSTY ���� ---------------------
    public void DecreaseTHIRSTY(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    //------------------- SATISFY ���� ---------------------
    public void IncreaseSATISFY(int _count)
    {
        if (currentSatisfy + _count < satisfy)
            currentSatisfy += _count;
        else
            currentSatisfy = satisfy;
    }

    //------------------- SATISFY ���� ---------------------
    public void DecreaseSATISFY(int _count)
    {
        if (currentSatisfy - _count < 0)
            currentSatisfy = 0;
        else
            currentSatisfy -= _count;
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
