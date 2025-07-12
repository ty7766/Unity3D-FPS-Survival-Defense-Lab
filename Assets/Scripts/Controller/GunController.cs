using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class GunController : MonoBehaviour
{
    public static bool isActivate = false;       //���� Ȱ��ȭ ����

    [Header("���� ������ ��")]
    [SerializeField]
    private Gun currentGun;

    private Vector3 originPos;              //���� ��ġ (������ X)
    private float currentFireRate;
    private bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;
    private RaycastHit hitInfo;             //�Ѿ˿� �ǰݵ� ������Ʈ

    [Header("���� ������Ʈ")]
    public Camera cam;
    public GameObject hit_effect_prefab;       //�ǰ� ����Ʈ
    public CrossHair crossHair;
    public PlayerController playerController;
    public LayerMask layerMask;

    private AudioSource audioSource;

    //�ʱ�ȭ
    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        crossHair = FindAnyObjectByType<CrossHair>();
    }

    void Update()
    {
        //���� ���Ⱑ "�Ǽ�"�� �� �ѿ� ���� ��ȣ�ۿ� ����
        if (isActivate)
        {
            GunFireRateCalculate();
            TryFire();
            TryReload();
            TryFineSight();
        }
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
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }
    //�߻� ��
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
    //�߻� ��
    private void Shoot()
    {
        //ũ�ν���� �ݵ� �ִϸ��̼� ����
        crossHair.FireAnimation();
        //���� �Ѿ� ���� ����
        currentGun.currentBulletCount--;
        //����ӵ� ����
        currentFireRate = currentGun.fireRate;
        //���� ����
        PlaySoundEffect(currentGun.fireSound);
        //����Ʈ ����
        currentGun.muzzleFlash.Play();
        //�ǰ� ����
        Hit();
        //�ѱ� �ݵ� ����
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }


    //-------------------------- �ǰ� ���� -------------------------
    private void Hit()
    {
        //Ray�� �࿡�� ���� ����� ���� -> �ݵ� ����
        //�Ѿ��� ���ư��� ������ ������ �ٸ� ���� ���� Random�� �̿��Ͽ� ����
        if(Physics.Raycast(cam.transform.position, cam.transform.forward +
            new Vector3(Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy),        //x�� �ݵ�
                        Random.Range(-crossHair.GetAccuracy() - currentGun.accuracy, crossHair.GetAccuracy() + currentGun.accuracy),        //y�� �ݵ�
                        0)
            //fix. �þ߰� �������� ���� �÷��̾��� ���̾ Player�� �Ǿ� CloseWeapon���� �ڱ� �ڽ��� �ǰݵǴ� �� ����
            , out hitInfo, currentGun.range, layerMask))
        {
            //ǥ���� �ٶ󺸴� ���⿡ ���� �ǰ� ����Ʈ�� ������ ����
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //����Ʈ ������ �� ���� clone�� 1.5�� �� ����
            Destroy(clone, 1.5f);
        }
    }


    //--------------------------- ������ ----------------------
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();      //������ ���߿� ������ �� ������ ��带 Ǯ�� ������ ����
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;       //�Ѿ��� �����ִ� ���·� ������ �� ���� �Ѿ˼� �°�
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            //�������� �� �ִ� �ִ� �������� ���� �� ������ �ִ� �Ѿ� ���� ������ �ִ�ġ�� ������
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            //�������� �� �ִ� �ִ� �������� ���� �� ������ �ִ� �Ѿ� ���� ������ ���� �Ѿ� ����ŭ ������
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("������ �Ѿ��� ����!");
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


    //------------------- �Ѱ� ���� ��� -----------------
    private void PlaySoundEffect(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }


    //------------------------- ������ ------------------------
    private void TryFineSight()
    {
        //������ ���߿� ������ ����
        if(Input.GetButton("Fire2") && !isReload)
        {
            FineSight();
        }
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;             //���� Ȱ��ȭ/��Ȱ��ȭ
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        crossHair.FineSightAnimation(isFineSightMode);                 //������ ũ�ν���� ����

        //�⺻ �����϶� ������ Ŭ�� -> ������ �������� ����
        if (isFineSightMode)
        {
            StopAllCoroutines();        //�ڷ�ƾ �Լ��� �ߺ� ����
            StartCoroutine(FineSightActivateCoroutine());
        }
        //������ �����϶� ������ Ŭ�� -> ������ ����
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

    //������ Ȱ��
    IEnumerator FineSightActivateCoroutine()
    {
        //������ ��ġ ������  ���� ��ġ ������ �������� ���� �ݺ�
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    //������ ��Ȱ��
    IEnumerator FineSightDeactivateCoroutine()
    {
        //������ ��ġ ������  ���� ��ġ ������ �������� ���� �ݺ�
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }


    //-------------------------- �ѱ� �ݵ� -----------------------------
    IEnumerator RetroActionCoroutine()
    {
        //���� 90�� �����ֱ� ������ x���� Ȱ���Ͽ� �ݵ� �ֱ�
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z) ;
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        //���� ��ġ -> �ݵ� ��ġ -> ���� ��ġ
        if(!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            //�ݵ� ����
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)      //���� ����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            //�ݵ� �� ����ġ
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        //������ ��ġ -> �ݵ� ��ġ -> ���� ��ġ
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            //�ݵ� ����
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)      //���� ����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            //�ݵ� �� ����ġ
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }


    //----------------------------- ���� ��ü ----------------------------
    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);        //���� ���� �Ⱥ��̰� �ϱ�

        currentGun = _gun;                                                  //���� ���⸦ ���� ����� ����
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>(); //���� ������ ������Ʈ ����
        WeaponManager.currentWeaponAnimator = currentGun.anim;              //���� ������ �ִϸ��̼� ����

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);                              //���� ���� ���̰� �ϱ�

        isActivate = true;
    }


    //HUD�� ��ȯ �Լ�
    public Gun GetGun()
    {
        return currentGun;
    }

    //CrossHair�� ��ȯ �Լ�
    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
}

