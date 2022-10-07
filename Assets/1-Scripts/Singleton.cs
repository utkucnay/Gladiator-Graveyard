using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class Singleton <T> : Receiver where T : MonoBehaviour
{

    /// <summary>
    /// In order to create a singleton class, derive it from this and call the base.Awake();
    /// To reference a singleton, call MyClass myClass = MyClass.Request();
    /// </summary>
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static T Request()
    {
        if (instance == null)
        {
            Debug.LogError("There is no instance of " + typeof(T).Name + " in the scene");
            Debug.Break();
        }
        return instance;
    }

    public static T ForceRequest()
    {
        if (instance == null)
        {
            GameObject newObject = new GameObject();
            instance = newObject.AddComponent<T>();
        }
        return instance;
    }
}
