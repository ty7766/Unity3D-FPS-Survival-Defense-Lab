using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    public GunController gunController;
    public GameObject go_BulletHUD;     //HUD Ȱ��/��Ȱ��
    public Text[] text_Bullet;

    private Gun currentGun;         //���� �� ����

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = gunController.GetGun();
        text_Bullet[0].text = currentGun.carryBulletCount.ToString();
        text_Bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
}
