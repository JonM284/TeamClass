using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class myPool
    {
        public string poolTag;
        public GameObject poolObject;
        public int total_Pool_Amt;
    }

    public static ObjectSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<myPool> objectPools;
    public Dictionary<string, Queue<GameObject>> objPoolDictionary;
    //this code was created using the help of brackeys on youtube

    // Use this for initialization
    void Start()
    {
        objPoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (myPool pool in objectPools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < pool.total_Pool_Amt; i++)
            {
                GameObject obj = Instantiate(pool.poolObject);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            objPoolDictionary.Add(pool.poolTag, objPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!objPoolDictionary.ContainsKey(tag))
        {
            return null;
        }
        GameObject obj_To_Spawn = objPoolDictionary[tag].Dequeue();

        obj_To_Spawn.SetActive(true);
        obj_To_Spawn.transform.position = position;
        obj_To_Spawn.transform.rotation = rotation;

        objPoolDictionary[tag].Enqueue(obj_To_Spawn);

        return obj_To_Spawn;
    }
}
