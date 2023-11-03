using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSound : MonoBehaviour
{
    public static ShootSound Instance;
    private AudioSource m_AudioSource;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        m_AudioSource = GetComponent<AudioSource>();
    }
    public void PlayShoot()
    {
        m_AudioSource.time = 0;
        m_AudioSource.Play();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

}
