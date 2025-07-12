using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [Header("연결 컴포넌트")]
    public Animator animator;
    public GameObject go_CrossHairHUD;          //크로스헤어 활성화/비활성화
    public GunController gunController;

    private float gunAccuracy;      //정확도

    //------------------- 크로스헤어 애니메이션 재생 -----------------------
    public void WalkingAnimation(bool _flag)
    {
        if(!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnimator.SetBool("Walk", _flag);
            animator.SetBool("Walking", _flag);
        }
    }
    public void RunningAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            WeaponManager.currentWeaponAnimator.SetBool("Run", _flag);
            animator.SetBool("Running", _flag);
        }
    }
    public void JumpAnimation(bool _flag)
    {
        if (!GameManager.isWater)
            animator.SetBool("Running", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        if (!GameManager.isWater)
            animator.SetBool("Crouching", _flag);
    }
    public void FineSightAnimation(bool _flag)
    {
        if (!GameManager.isWater)
            animator.SetBool("FineSight", _flag);
    }
    public void FireAnimation()
    {
        if (!GameManager.isWater)
        {
            if (animator.GetBool("Walking"))
                animator.SetTrigger("Walk_Fire");
            else if (animator.GetBool("Crouching"))
                animator.SetTrigger("Crouch_Fire");
            else
                animator.SetTrigger("Idle_Fire");
        }
    }

    //--------------------- 반동 수치 설정 ----------------------
    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
            gunAccuracy = 0.06f;
        else if (animator.GetBool("Crouching"))
            gunAccuracy = 0.01f;
        else if (gunController.GetFineSightMode())
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;

        return gunAccuracy;
    }
}
