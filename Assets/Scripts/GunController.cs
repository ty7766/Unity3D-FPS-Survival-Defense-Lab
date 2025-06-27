using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;         //���� �Ӽ�

    private float currentFireRate;
    
    private AudioSource audioSource;


    //�ʱ�ȭ
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

    //-------------------- ���� �߻� �ð� ó�� �޼ҵ� -------------------
    private void GunFireRateCalculate()
    {
        //������ 0�� �Ǹ� �ٽ� �߻� ����
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;      //1�ʿ� 1 ����
    }

    //------------------- �߻� �޼ҵ� -----------------------
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
        //���� ����
        PlaySoundEffect(currentGun.fireSound);
        //����Ʈ ����
        currentGun.muzzleFlash.Play();

        Debug.Log("�Ѿ� �߻�!");
    }

    //------------------- �Ѱ� ���� ��� -----------------
    private void PlaySoundEffect(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
