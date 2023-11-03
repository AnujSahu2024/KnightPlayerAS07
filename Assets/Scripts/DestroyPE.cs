using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPE : MonoBehaviour
{
    private ParticleSystem pe;
    // Start is called before the first frame update
    void Start()
    {
        pe = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pe.isStopped)
        {
            Destroy(gameObject);
        }
    }
}
