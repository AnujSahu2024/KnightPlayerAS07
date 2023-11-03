using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObsticalManager : MonoBehaviourPunCallbacks
{
    public static ObsticalManager Instance;
    public GameObject obsticals;
    private int obsticalCount;
    private float obsY = 1.4f, obsX, obsZ;
    public List<int> listPosX = new List<int>();
    public List<int> listPosZ = new List<int>();
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        Debug.LogError("Obstical Manager called  ");
        obsticalCount = 5;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber==1)
        {
            //if(photonView.IsMine)
            //{
                ObsticaleSpanner();
                ShowPosition();
           
                ///Obsinstantiate();
           // }
            
        }

    }
    private void Obsinstantiate()
    {
        for(int i=0;i<listPosX.Count;i++)
        {
            Vector3 pp = new Vector3((float)listPosX[i],1.4f,(float)listPosZ[i]);
           GameObject obs = PhotonNetwork.Instantiate(obsticals.name, pp, Quaternion.identity);
            obs.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }
       
    }
    private void ShowPosition()
    {
        for(int i=0;i<listPosX.Count;i++)
        {
            Debug.LogFormat("<color=yellow> X : {0} Z : {1} </color>",listPosX[i],listPosZ[i]);
        }
    }
    private void GetUniquePos()
    {
        obsX = Random.RandomRange(-3.5f, 3.5f);
        obsZ = Random.RandomRange(0f, 6f);
        if(listPosX.Contains((int)obsX) || listPosZ.Contains((int)obsZ))
        {
            GetUniquePos();
        }
        else
        {
            listPosX.Add((int)obsX);
            listPosZ.Add((int)obsZ);
        }
    }
    private void ObsticaleSpanner()
    {
        for(int i=0;i<obsticalCount;i++)
        {

            GetUniquePos();
        }
       
    }
}
