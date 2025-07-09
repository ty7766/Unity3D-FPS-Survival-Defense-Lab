using UnityEngine;

/// <summary>
/// Animal에서 상속
/// </summary>
public class WeakAnimal : Animal
{
    //------------------------------------------ 도망가기 메소드 ---------------------------------------
    //공격자에게 공격을 받은 순간 공격자의 반대 방향으로 뛰기
    public void Run(Vector3 _targetPos)
    {
        //뛰는 방향을 공격자의 반대방향으로 설정
        direc = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;
        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);

    }
    public override void Damage(int _dmg, Vector3 _targetPos)
    {
        base.Damage(_dmg, _targetPos);
        if (!isDead)
            Run(_targetPos);
    }
}
