using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<T>();
            
            if (_instance == null)
            {
                GameObject prefab = Resources.Load(typeof(T).Name) as GameObject;
                GameObject singleton = Instantiate(prefab);
                _instance = singleton.GetComponent<T>();
            }
            
            return _instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
    }
}
