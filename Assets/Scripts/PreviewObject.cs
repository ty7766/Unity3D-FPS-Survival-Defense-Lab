using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField]
    private int layerGround;            //Ground 레이어
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    private List<Collider> colliderList = new List<Collider>();         //충돌한 오브젝트의 Collider

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);
    }
    

    //----------------------------설치 가능 지형 유무 판단 메소드------------------------------------
    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
    }


    //----------------------------    색 변경     ------------------------------------
    private void SetColor(Material material)
    {
        //모든 자식 객체(모닥불을 구성하는 나무들)에 색 변경 적용
        foreach(Transform child in this.transform)
        {
            var newMaterials = new Material[child.GetComponent<Renderer>().materials.Length];

            for(int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = material;
            }

            child.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}
