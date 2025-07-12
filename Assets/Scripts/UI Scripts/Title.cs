using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "GameStage";        //씬 이름

    private SaveAndLoad saveAndLoad;

    public static Title Instance;

    //싱글톤
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

    //----------------------------- 씬 체인지 --------------------------------
    public void ClickStart()
    {
        Debug.Log("로딩");
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("로드");
        //모든 오브젝트가 불러와질 때까지 대기 후 로드
        StartCoroutine(LoadCoroutine());
    }
    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    IEnumerator LoadCoroutine()
    {
        //동기화
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
