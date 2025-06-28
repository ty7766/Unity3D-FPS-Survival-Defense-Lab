using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunController : MonoBehaviour
{
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
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject hit_effect_prefab;       //피격 이펙트

    private AudioSource audioSource;

    //초기화
    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalculate();
        TryFire();
        TryReload();
        TryFineSight();
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
        if (!isReload)
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
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, currentGun.range))
        {
            //표면이 바라보는 방향에 따라서 피격 이펙트의 방향을 적용
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //이펙트 적용이 다 끝난 clone은 1.5초 후 삭제
            Destroy(clone, 1.5f);
        }
    }

    //--------------------------- 수동 재장전 메소드 ----------------------
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();      //정조준 도중에 재장전 시 정조준 모드를 풀고 재장전 진행
            StartCoroutine(ReloadCoroutine());
        }
    }

    //------------------- 총격 사운드 재생 -----------------
    private void PlaySoundEffect(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    //------------------- 자동 재장전 메소드 -------------------
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
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


    //HUD용 반환 함수
    public Gun GetGun()
    {
        return currentGun;
    }
}

