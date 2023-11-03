using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Loboanime : MonoBehaviour
{
    public GameObject StartPos, DestPos,PhotonConnector;
    private AudioSource audioClipSward;
    // Start is called before the first frame update
    private void Awake()
    {
        transform.position = StartPos.transform.position;
    }
    void Start()
    {
       
        MoveLobo();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(transform.position);
        }
    }
    public void MoveLobo()
    {
        audioClipSward=GetComponent<AudioSource>();
        transform.DOMove(DestPos.transform.position, 0.4f).SetEase(Ease.InOutSine).OnStart(() => {
            audioClipSward.Play();
        }).OnComplete(() => {
            PhotonConnector.SetActive(true);
        });
    }


}
