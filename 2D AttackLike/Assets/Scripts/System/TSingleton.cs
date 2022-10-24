using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T> : MonoBehaviour where T : TSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject newObject = new GameObject(typeof(T).Name, typeof(T));
                instance = newObject.AddComponent<T>();

                //DontDestroyOnLoad(newObject);
            }

            return instance;
        }
    }
}
