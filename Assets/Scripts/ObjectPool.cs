using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // List of game object without "(Clone)", refer this list to get the gameObject you want to initilise
    public List<GameObject> objectTemplate = new List<GameObject>();


    // key: String object name without "(Clone)"
    // value: stores a list of objects which was already initialised
    private Dictionary<string, List<GameObject>> objectPool =
        new Dictionary<string, List<GameObject>>();


    private void Start()
    {
        foreach (GameObject go in objectTemplate)
            Put(go.name, new List<GameObject>());
    }

    public GameObject SpawnObject(string key)
    {
        GameObject toSpawn;
        List<GameObject> tempList = GetObjectList(key);

        // if list has anything -> list has something
        if (tempList.Any())
        {
            toSpawn = tempList[0];
            tempList.RemoveAt(0);
            toSpawn.SetActive(true);
            return toSpawn;
        }
        else
        {
            toSpawn = Instantiate(GetOriginalObject(key));
            toSpawn.transform.parent = transform;
            return toSpawn;
        }

    }


    public void PutBackInPool(GameObject objectToPutBack)
    {
        string key = TrimWordClone(objectToPutBack.name);
        objectToPutBack.SetActive(false);

        List<GameObject> tempList = GetObjectList(key);
        tempList.Add(objectToPutBack);
        Put(key, tempList);

        //Debug.Log("Put back into pool succeed" + key);
    }


    //  Used by powerups so they would not be destroyed before the coroutine
    public IEnumerator LatePutBackInPool(GameObject objectToPutBack, float time)
    {
        yield return new WaitForSeconds(time);
        PutBackInPool(objectToPutBack);
    }


    //-----------------Private methods---------------------------------------


    // Encapsulation to make it like java hashmap
    private void Put(string key, List<GameObject> value)
    {
        objectPool[key] = value;
    }


    // Gets object list in the dictionary
    private List<GameObject> GetObjectList(string key)
    {
        CheckIsValidKey(key);
        return objectPool[key];
    }


    private GameObject GetOriginalObject(string key)
    {
        CheckIsValidKey(key);
        foreach (GameObject go in objectTemplate)
            if (go.name.Equals(key)) return go;
        return null;
    }


    private void CheckIsValidKey(string key)
    {
        try
        {
            var testing = objectPool[key];
        }
        catch
        {
            Debug.LogError("ObjectPool--- key is not in objectPool: " + key);
        }
    }

    private string TrimWordClone(string key)
    {
        return key.Remove(key.Length - 7);
    }



}