using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceManager : MonoBehaviour
{
    private static InstanceManager _instance = null;

    private void Awake()
    {
        if (null == _instance)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public static InstanceManager Instance
    {
        get
        {
            if (null == _instance)
            {
                return null;
            }

            return _instance;
        }
    }
}
