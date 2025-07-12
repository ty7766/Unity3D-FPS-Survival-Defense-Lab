using UnityEngine;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;

//������ ������ ���� ����ȭ
//�����͵��� �� �ٷ� �����Ǿ� �б�/���Ⱑ ������
[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;           //����� �÷��̾� ��ġ
    public Vector3 playerRotation;      //����� �÷��̾ ���� ����

    //�κ��丮 ����
    public List<int> inventoryArrayNum = new List<int>();
    public List<string> inventoryItemName = new List<string>();
    public List<int> inventoryItemNum = new List<int>();
}
public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();
    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController playerController;
    private Inventory inventory;

    void Start()
    {
        //Saves ������ ���� ���� ��� ����
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        //�÷��̾� ã��
        playerController = FindAnyObjectByType<PlayerController>();
        //�κ��丮 ã��
        inventory = FindAnyObjectByType<Inventory>();

        //�÷��̾� ���� ����
        saveData.playerPos = playerController.transform.position;
        saveData.playerRotation = playerController.transform.eulerAngles;

        //�κ��丮 ���� ����
        Slot[] slots = inventory.GetSlots();
        for(int i = 0; i <slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.inventoryArrayNum.Add(i);
                saveData.inventoryItemName.Add(slots[i].item.itemName);
                saveData.inventoryItemNum.Add(slots[i].itemCount);
            }
        }

        //���̺� ������ .json���� ��ȯ
        string json = JsonUtility.ToJson(saveData);
        //������ ��� �ؽ�Ʈ�� ����
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("���� �Ϸ�");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            //���̺� ������ ��� ���� �б�
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            //Save�Ǿ��ִ� Json������ �ٽ� ����Ƽ ���Ϸ� Ǯ��
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            //�÷��̾� ã��
            playerController = FindAnyObjectByType<PlayerController>();
            //�κ��丮 ã��
            inventory = FindAnyObjectByType<Inventory>();

            //����Ǿ��ִ� �÷��̾� �Ӽ� ����
            playerController.transform.position = saveData.playerPos;
            playerController.transform.eulerAngles = saveData.playerRotation;
            //����Ǿ��ִ� �κ��丮 ����
            for(int i = 0; i < saveData.inventoryItemName.Count; i++)
            {
                inventory.LoadToInventory(saveData.inventoryArrayNum[i], saveData.inventoryItemName[i], saveData.inventoryItemNum[i]);
            }

            Debug.Log("�ε� �Ϸ�");
        }
        else
            Debug.Log("���̺� ������ �����ϴ�!");
    }
}
