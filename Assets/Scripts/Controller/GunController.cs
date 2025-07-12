using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunController : MonoBehaviour
{
    public static bool isActivate = false;       //무기 활성화 여부

    [Header("현재 장착된 총")]
    [SerializeField]
    private Gun currentGun;

    private Vector3 originPos;              //원래 위치 (정조준 X)
    private float currentFireRate;
    private bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;
    private RaycastHit hitInfo;             //총알에 피격된 오브젝트

    [Header("연결 컴포넌트")]
    public Camera cam;
    public GameObject hit_effect_prefab;       //피격 이펙트
    public CrossHair crossHair;
    public PlayerController playerController;
    public LayerMask layerMask;

    private AudioSource audioSource;

    //초기화
    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        crossHair = FindAnyObjectByType<CrossHair>();
    }

    void Update()
    {
        //현재 무기가 "맨손"일 때 총에 대한 상호작용 금지
        if (isActivate)
        {
            GunFireRateCalculate();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    //-------------------- 총의 발사 시간 처리 메소드 -------------------
    private void GunFireRateCalculate()
    {
        //변수가 0이 되면 다시 발사 가능
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;      //1초에 1 감소
    }


    //------------------- 발사 메소드 -----------------------
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }
    //발사 전
    private void Fire()
    {
        if (!isReload && !playerController.isRun)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
                StartCoroutine(ReloadCoroutine());
        }
    }
    //발사 후
    private void Shoot()
    {
        //크로스헤어 반동 애니메이션 적용
        crossHair.FireAnimation();
        //현재 총알 개수 감소
        currentGun.currentBulletCount--;
        //연사속도 재계산
        currentFireRate = currentGun.fireRate;
        //사운드 적용
        PlaySoundEffect(currentGun.fireSound);
        //이펙트 적용
        currentGun.muzzleFlash.Play();
        //피격 적용
        Hit();
        //총기 반동 적용
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }


    //-------------------------- 피격 적용 -------------------------
    private void Hit()
    {
        //Ray를 축에서 조금 벗어나게 설정 -> 반동 설정
        //총알이 나아가는 방향을 제외한 다른 방향 축을 Random을 이용하여 흔들기
        if(Physics.Raycast(cam.transform.position, cam.transform.forward +
            new Vector3(Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy),        //x축 반동
                        Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy),        //y축 반동
                        0)
            //fix. 시야각 구현으로 인해 플레이어의 레이어가 Player로 되어 CloseWeapon으로 자기 자신이 피격되는 것 방지
            , out hitInfo, currentGun.range, layerMask))
        {
            //표면이 바라보는 방향에 따라서 피격 이펙트의 방향을 적용
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //이펙트 적용이 다 끝난 clone은 1.5초 후 삭제
            Destroy(clone, 1.5f);
        }
    }


    //--------------------------- 재장전 ----------------------
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();      //정조준 도중에 재장전 시 정조준 모드를 풀고 재장전 진행
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;       //총알이 남아있는 상태로 재장전 시 기존 총알수 승계
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            //재장전할 수 있는 최대 개수보다 내가 총 가지고 있는 총알 수가 많으면 최대치를 재장전
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            //재장전할 수 있는 최대 개수보다 내가 총 가지고 있는 총알 수가 적으면 남은 총알 수만큼 재장전
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없음!");
        }
    }
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }


    //------------------- 총격 사운드 재생 -----------------
    private void PlaySoundEffect(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //------------------------- 정조준 ------------------------
    private void TryFineSight()
    {
        //재장전 도중에 정조준 방지
        if(Input.GetButton("Fire2") && !isReload)
        {
            FineSight();
        }
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;             //조준 활성화/비활성화
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        crossHair.FineSightAnimation(isFineSightMode);                 //정조준 크로스헤어 적용

        //기본 상태일때 정조준 클릭 -> 정조준 시점으로 변경
        if (isFineSightMode)
        {
            StopAllCoroutines();        //코루틴 함수의 중복 방지
            StartCoroutine(FineSightActivateCoroutine());
        }
        //정조준 시점일때 정조준 클릭 -> 원상태 복귀
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }

    }
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    //정조준 활성
    IEnumerator FineSightActivateCoroutine()
    {
        //정조준 위치 시점과  현재 위치 시점이 같아질때 까지 반복
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    //정조준 비활성
    IEnumerator FineSightDeactivateCoroutine()
    {
        //정조준 위치 시점과  현재 위치 시점이 같아질때 까지 반복
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }


    //-------------------------- 총기 반동 -----------------------------
    IEnumerator RetroActionCoroutine()
    {
        //총이 90도 꺾여있기 때문에 x축을 활용하여 반동 주기
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z) ;
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //원래 위치 -> 반동 위치 -> 원래 위치
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            //반동 시작
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)      //러프 보정
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //반동 후 원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        //정조준 위치 -> 반동 위치 -> 원래 위치
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //반동 시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)      //러프 보정
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //반동 후 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }


    //----------------------------- 무기 교체 ----------------------------
    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);        //현재 무기 안보이게 하기

        currentGun = _gun;                                                  //다음 무기를 현재 무기로 설정
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>(); //다음 무기의 오브젝트 적용
        WeaponManager.currentWeaponAnimator = currentGun.anim;              //다음 무기의 애니메이션 적용

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);                              //다음 무기 보이게 하기

        isActivate = true;
    }


    //HUD용 반환 함수
    public Gun GetGun()
    {
        return currentGun;
    }

    //CrossHair용 반환 함수
    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
}

