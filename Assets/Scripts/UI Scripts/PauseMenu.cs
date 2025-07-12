using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject go_BaseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!GameManager.isPause)
                CallMenu();
            else
                CloseMenu();
        }
    }

    private void CallMenu()
    {
        GameManager.isPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CloseMenu()
    {
        GameManager.isPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ClickSave()
    {
        Debug.Log("���̺�");
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }
}
