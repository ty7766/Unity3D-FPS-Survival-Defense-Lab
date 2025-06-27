using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;         //총의 속성

    private float currentFireRate;
    
    private AudioSource audioSource;


    //초기화
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalculate();
        TryFire();
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
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        //사운드 적용
        PlaySoundEffect(currentGun.fireSound);
        //이펙트 적용
        currentGun.muzzleFlash.Play();

        Debug.Log("총알 발사!");
    }

    //------------------- 총격 사운드 재생 -----------------
    private void PlaySoundEffect(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
