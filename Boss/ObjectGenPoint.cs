using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenPoint : MonoBehaviour
{
    public GameObject objPrefab;        // 発生させるPrefabデータ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectCreate()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, -1.0f);
        // PrefabからGameObjectを作る
        Instantiate(objPrefab, pos, Quaternion.identity);
    }
}
