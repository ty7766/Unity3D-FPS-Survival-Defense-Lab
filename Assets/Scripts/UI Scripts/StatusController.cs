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
    private int spIncreaseSpeed;            //SP 증가 속도
    [SerializeField]
    private int spRechargeTime;             //SP 회복 딜레이
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
    private int hungryDecreaseTime;                     //배고픔 감소 시간
    private int currentHungryDecreaseTime;

    [Header("THIRSTY")]
    [SerializeField]
    private int thirsty;
    private int currentThirsty;
    [SerializeField]
    private int thirstyDecreaseTime;                    //목마름 감소 시간
    private int currentThirstyDecreaseTime;

    [Header("SATISFY")]
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    [Header("연결 컴포넌트")]
    [SerializeField]
    private Image[] images_Gauge;

    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;         //Status 번호 부여


    //초기화
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

    //---------------------------- 배고픔 수치 ----------------------------
    private void Hungry()
    {
        if (currentHungry > 0)
        {
            //배고픔 감소 시간이 설정한 시간에 도달하면 배고픔 수치를 1개씩 감소시키기
            if (currentHungryDecreaseTime <= hungryDecreaseTime)
                currentHungryDecreaseTime++;
            else
            {
                currentHungry--;
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다!");
    }

    //------------------------------ 갈증 수치 -----------------------------
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            //배고픔 감소 시간이 설정한 시간에 도달하면 배고픔 수치를 1개씩 감소시키기
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime)
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("갈증 수치가 0이 되었습니다!");
    }

    //------------------------ UI 게이지 감소 -----------------------------
    private void GaugeUpdate()
    {
        images_Gauge[HP].fillAmount = (float)currentHp / hp;
        images_Gauge[DP].fillAmount = (float)currentDp / dp;
        images_Gauge[SP].fillAmount = (float)currentSp / sp;
        images_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        images_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    //------------------------- SP 수치 --------------------------------
    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }
    //SP 회복 대기 시간
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
    //SP 회복
    private void SPRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    //------------------- HP 증가 ---------------------
    public void IncreaseHP(int _count)
    {
        if (currentHp + _count < hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    //------------------- HP 감소 ---------------------
    public void DecreaseHP(int _count)
    {
        //DP가 있는 경우 DP 먼저 감소
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }

        currentHp -= _count;

        if (currentHp <= 0)
            Debug.Log("캐릭터의 hp가 0이 되었습니다!");
    }


    //------------------- SP 증가 ---------------------
    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;
    }

    //------------------- SP 감소 ---------------------
    public void DecreaseSP(int _count)
    {
        currentSp -= _count;

        if (currentSp <= 0)
            Debug.Log("캐릭터의 sp가 0이 되었습니다!");
    }


    //------------------- DP 증가 ---------------------
    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    //------------------- DP 감소 ---------------------
    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("캐릭터의 dp가 0이 되었습니다!");
    }


    //------------------- HUNGRY 증가 ---------------------
    public void IncreaseHUNGRY(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    //------------------- HUNGRY 감소 ---------------------
    public void DecreaseHUNGRY(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }


    //------------------- THIRSTY 증가 ---------------------
    public void IncreaseTHIRSTY(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    //------------------- THIRSTY 감소 ---------------------
    public void DecreaseTHIRSTY(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    //------------------- SATISFY 증가 ---------------------
    public void IncreaseSATISFY(int _count)
    {
        if (currentSatisfy + _count < satisfy)
            currentSatisfy += _count;
        else
            currentSatisfy = satisfy;
    }

    //------------------- SATISFY 감소 ---------------------
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
