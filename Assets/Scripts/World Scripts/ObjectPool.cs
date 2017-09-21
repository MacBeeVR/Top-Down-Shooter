using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    //Pooled Item Properties
    [Serializable]
    public class PooledItem
    {
        public GameObject pooledItem;
        public GameObject hierarchy;
        public string tag;
        public float numItemsToPool;
        public bool canResize;

        [NonSerialized]       
        public List<GameObject> pool;

        //Builds the Pool.
        public void buildPool()
        {
            pool = new List<GameObject>();
            for (int i = 0; i < numItemsToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate<GameObject>(pooledItem);
                obj.SetActive(false);
                obj.transform.parent = hierarchy.transform;
                pool.Add(obj);
            }
        }

        //Returns available item. if no available item found, either resize or return null.
        public GameObject getAvailableItem()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                    return pool[i];
            }

            if (canResize)
                return resizePool();
            else
                return null;

        }

        //Puts together all of the pooled objects in currently in use into a list and returns that list.
        public List<GameObject> getItemsInUse()
        {
            List<GameObject> activeItems = new List<GameObject>();
            for(int i = 0; i < pool.Count; i++)
            {
                if (pool[i].activeInHierarchy)
                    activeItems.Add(pool[i]);
            }
            return activeItems;
        }

        //Increases Pool Size and Returns the Newly Created Pooled Object for Use.
        //Use Under the Condition that no Other Pooled Objects are Available and Another is Needed.
        public GameObject resizePool()
        {
            GameObject obj = (GameObject)Instantiate<GameObject>(pooledItem);
            obj.SetActive(false);
            pool.Add(obj);
            obj.transform.parent = hierarchy.transform.parent;
            return obj;
        }
    }

    //Pool Code
    public List<PooledItem> itemsPool;

    void Start()
    {
        foreach(PooledItem item in itemsPool)
        {
            item.buildPool();
        }
    }

    //Get pooled object that corresponds to the itemTag.
    public GameObject getPooledItem(string itemTag)
    {
        for(int i = 0; i < itemsPool.Count; i++)
        {
            if (itemsPool[i].tag.Equals(itemTag))
                return itemsPool[i].getAvailableItem();
        }

        return null;
    }

    //Returns all active pooled objects of the item that matches itemTag.
    public List<GameObject> getActiveObjects(string itemTag)
    {
        List<GameObject> itemList = new List<GameObject>();
        for(int i = 0; i < itemsPool.Count; i++)
        {
            if (itemsPool[i].tag.Equals(itemTag))
                itemList = itemsPool[i].getItemsInUse();
        }

        if (itemList.Count > 0)
            return itemList;
        else
            return null;
    }
}
