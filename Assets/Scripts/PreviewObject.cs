using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField]
    private int layerGround;            //Ground ���̾�
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField]
    private Material green;
    [SerializeField]
    private Material red;

    private List<Collider> colliderList = new List<Collider>();         //�浹�� ������Ʈ�� Collider

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
    

    //----------------------------��ġ ���� ���� ���� �Ǵ� �޼ҵ�------------------------------------
    private void ChangeColor()
    {
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
    }


    //----------------------------    �� ����     ------------------------------------
    private void SetColor(Material material)
    {
        //��� �ڽ� ��ü(��ں��� �����ϴ� ������)�� �� ���� ����
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
