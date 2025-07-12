using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";        //�� �̸�

    private SaveAndLoad saveAndLoad;

    public static Title Instance;

    //�̱���
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    //----------------------------- �� ü���� --------------------------------
    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("�ε�");
        //��� ������Ʈ�� �ҷ����� ������ ��� �� �ε�
        StartCoroutine(LoadCoroutine());
    }
    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }

    IEnumerator LoadCoroutine()
    {
        //����ȭ
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            yield return null;
        }

        saveAndLoad = FindAnyObjectByType<SaveAndLoad>();
        saveAndLoad.LoadData();
        gameObject.SetActive(false);
    }
}
