using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbySound : MonoBehaviour
{
    private static lobbySound Instance; 
   // public Gameobject SoundPrefab;
    private void Awake()
    {
        Debug.Log("Awake called ");
       if(Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
       else
        {
            Destroy(this.gameObject);
        }
    }
}
