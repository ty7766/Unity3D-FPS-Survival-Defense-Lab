using UnityEngine;

public class Rock : MonoBehaviour
{

    [Header("바위 속성")]
    [SerializeField]
    private int hp;                 //바위의 체력
    [SerializeField]
    private float destroyTime;      //바위 파편이 없어지는 시간

    [Header("연결 컴포넌트")]
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private GameObject go_rock;                     //일반 바위 오브젝트
    [SerializeField]
    private GameObject go_debris;                   //조각 바위 오브젝트
    [SerializeField]
    private GameObject go_effect_prefab;            //채굴 이펙트

    [Header("필요 사운드")]
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        //채굴 사운드
        SoundManager.instance.PlaySoundEffects(strike_Sound);
        //채굴 이펙트 생성
        var clone = Instantiate(go_effect_prefab, sphereCollider.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime / 3);

        hp--;
        if (hp <= 0)
            Destruction();
    }

    //----------------------- 바위 파괴 -------------------------
    private void Destruction()
    {
        //파괴 사운드
        SoundManager.instance.PlaySoundEffects(destroy_Sound);

        //일반 바위 삭제 -> 조각 바위 활성 -> 조각 바위 삭제
        sphereCollider.enabled = false;
        Destroy(go_rock);
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
