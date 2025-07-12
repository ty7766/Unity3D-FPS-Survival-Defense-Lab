using UnityEngine;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;

//데이터 저장을 위한 직렬화
//데이터들이 한 줄로 나열되어 읽기/쓰기가 쉬워짐
[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;           //저장될 플레이어 위치
    public Vector3 playerRotation;      //저장될 플레이어가 보는 방향

    //인벤토리 정보
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
        //Saves 폴더에 저장 파일 경로 생성
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        //플레이어 찾기
        playerController = FindAnyObjectByType<PlayerController>();
        //인벤토리 찾기
        inventory = FindAnyObjectByType<Inventory>();

        //플레이어 상태 저장
        saveData.playerPos = playerController.transform.position;
        saveData.playerRotation = playerController.transform.eulerAngles;

        //인벤토리 상태 저장
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

        //세이브 파일을 .json으로 변환
        string json = JsonUtility.ToJson(saveData);
        //파일의 모든 텍스트를 저장
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            //세이브 파일의 모든 내용 읽기
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            //Save되어있는 Json파일을 다시 유니티 파일로 풀기
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            //플레이어 찾기
            playerController = FindAnyObjectByType<PlayerController>();
            //인벤토리 찾기
            inventory = FindAnyObjectByType<Inventory>();

            //저장되어있던 플레이어 속성 복구
            playerController.transform.position = saveData.playerPos;
            playerController.transform.eulerAngles = saveData.playerRotation;
            //저장되어있던 인벤토리 복구
            for(int i = 0; i < saveData.inventoryItemName.Count; i++)
            {
                inventory.LoadToInventory(saveData.inventoryArrayNum[i], saveData.inventoryItemName[i], saveData.inventoryItemNum[i]);
            }

            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다!");
    }
}
