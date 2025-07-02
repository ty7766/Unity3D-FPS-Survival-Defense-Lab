using UnityEngine;

//-------------------- 곡 속성 클래스 -----------------------
[System.Serializable]
public class Sound
{
    public string name;     //곡 이름
    public AudioClip clip;  //곡
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;            //Singleton

    #region singleton
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    [Header("연결 컴포넌트")]
    public AudioSource[] audioSourceEffects;        //사운드 이펙트들
    public AudioSource audioSourceBgm;              //배경음

    [Header("곡 관리")]
    public Sound[] effectSounds;
    public Sound[] bgmSounds;
    public string[] playSoundName;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    //-------------------------- 사운드 재생 --------------------------
    public void PlaySoundEffects(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for(int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.Log(_name + " 사운드가 SoundManager에 등록되지 않았습니다.");
    }

    //------------------------- 모든 사운드 Stop ----------------------
    public void StopAllSoundEffects()
    {
        for (int i = 0; i < audioSourceEffects.Length;i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    //------------------------- 특정 사운드 Stop -----------------------
    public void StopSoundEffects(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + _name + " 사운드가 없습니다.");

    }
}
