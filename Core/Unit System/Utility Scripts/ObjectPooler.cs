using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    [System.Serializable]
    public struct Pool {
        public string tag;
        public GameObject prefab;
        public Transform parentPool;
        public int size;
    }

    #region Singleton

    public static ObjectPooler instance;
    void Awake () {
        instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Use this for initialization
    void Start () {

        poolDictionary = new Dictionary<string, Queue<GameObject>> ();

        foreach (Pool pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject> ();
            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate (pool.prefab, pool.parentPool);
                obj.SetActive (false);
                objectPool.Enqueue (obj);
            }
            poolDictionary.Add (pool.tag, objectPool);
        }

    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation) {
        if (!poolDictionary.ContainsKey (tag)) {
            Debug.LogWarning ("Warning: Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue ();
        objectToSpawn.SetActive (true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject> ();

        if (pooledObject != null) {
            pooledObject.OnObjectSpawn ();
        }

        poolDictionary[tag].Enqueue (objectToSpawn);
        return objectToSpawn;
    }

}