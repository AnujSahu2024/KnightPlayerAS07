using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class waitingsc : MonoBehaviour
{
    public TextMeshProUGUI waiting;
    private string s1 = "Waiting for another.",s2= "Waiting for another..",s3= "Waiting for another...";
    // Start is called before the first frame update
    void Start()
    {
        itr();
    }
    public void itr()
    {
        if(gameObject.activeInHierarchy)
        {
            StartCoroutine(wait());
            itr();
        }
    }

    public IEnumerator wait()
    {
        
            waiting.text = s1.ToString();
            new WaitForSeconds(1f);
            waiting.text = s2.ToString();
            new WaitForSeconds(1f);
            waiting.text = s3.ToString();
            new WaitForSeconds(1f);

       
        yield return null;
    }
}
