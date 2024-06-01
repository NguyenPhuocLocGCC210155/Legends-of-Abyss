using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaObjectPooling : MonoBehaviour
{
    public GameObject objectPrefab;
    public int initialPoolSize = 10;
    private List<GameObject> pool;
    GameObject poolParent;
    void Start()
    {
        poolParent = new GameObject("ManaPool");
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
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }
}
