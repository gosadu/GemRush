using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generic pool for reusing game objects. No placeholders. 
/// Single dev can expand if synergy expansions or cameo illusions usage hooking VFX are spawned frequently.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class PoolItem
    {
        public string poolID;
        public GameObject prefab;
        public int initialCount;
        [HideInInspector] public Queue<GameObject> poolQueue= new Queue<GameObject>();
    }

    public List<PoolItem> items;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(var pi in items)
        {
            for(int i=0; i< pi.initialCount; i++)
            {
                var obj= Instantiate(pi.prefab, this.transform);
                obj.SetActive(false);
                pi.poolQueue.Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string poolID, Vector3 pos, Quaternion rot)
    {
        var pi= items.Find(x=>x.poolID== poolID);
        if(pi==null)
        {
            Debug.LogWarning($"[ObjectPool] No pool ID= {poolID}");
            return null;
        }
        if(pi.poolQueue.Count>0)
        {
            var obj= pi.poolQueue.Dequeue();
            obj.transform.position= pos;
            obj.transform.rotation= rot;
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var newObj= Instantiate(pi.prefab,pos,rot, this.transform);
            return newObj;
        }
    }

    public void ReturnToPool(string poolID, GameObject obj)
    {
        var pi= items.Find(x=>x.poolID== poolID);
        if(pi==null)
        {
            Destroy(obj);
            return;
        }
        obj.SetActive(false);
        pi.poolQueue.Enqueue(obj);
        obj.transform.SetParent(this.transform);
    }
}
