using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEManager : MonoBehaviour
{
    public GameObject ExplosionPE;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.transform.tag== "TestGround")
        {
            Vector3 pos = gameObject.transform.position;
            GameObject ob = Instantiate(ExplosionPE,pos,Quaternion.identity);
           // ExplosionPE.transform.position = gameObject.transform.position;
           // ExplosionPE.SetActive(true);
            Debug.LogError("Collide with Ground");
            Destroy(gameObject);
        }
    }
}
