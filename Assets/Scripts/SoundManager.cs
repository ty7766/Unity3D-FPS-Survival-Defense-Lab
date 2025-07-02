using UnityEngine;

//-------------------- �� �Ӽ� Ŭ���� -----------------------
[System.Serializable]
public class Sound
{
    public string name;     //�� �̸�
    public AudioClip clip;  //��
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

    [Header("���� ������Ʈ")]
    public AudioSource[] audioSourceEffects;        //���� ����Ʈ��
    public AudioSource audioSourceBgm;              //�����

    [Header("�� ����")]
    public Sound[] effectSounds;
    public Sound[] bgmSounds;
    public string[] playSoundName;

    void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    //-------------------------- ���� ��� --------------------------
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
                Debug.Log("��� ���� AudioSource�� ������Դϴ�");
                return;
            }
        }
        Debug.Log(_name + " ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    //------------------------- ��� ���� Stop ----------------------
    public void StopAllSoundEffects()
    {
        for (int i = 0; i < audioSourceEffects.Length;i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    //------------------------- Ư�� ���� Stop -----------------------
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
        Debug.Log("��� ����" + _name + " ���尡 �����ϴ�.");

    }
}
