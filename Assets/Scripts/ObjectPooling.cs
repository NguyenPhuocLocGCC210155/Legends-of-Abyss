using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject objectPrefab;
    public int initialPoolSize = 5;
    private List<GameObject> pool;
    GameObject poolParent;
    void Start()
    {
        poolParent = new GameObject("Pool");
        pool = new List<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, poolParent.transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = transform.position;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(objectPrefab, poolParent.transform);
        newObj.transform.position = transform.position;
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }
}
