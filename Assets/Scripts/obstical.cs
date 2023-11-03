using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class obstical : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    internal PhotonView obspv;
    public GameObject ExplosionPE;
    void Start()
    {
        obspv = GetComponent<PhotonView>();   
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ball collide with obstical");
        if(other.transform.tag=="ball")
        {
            // if (obspv.IsMine)
            //{
            Vector3 pos = gameObject.transform.position;
            GameObject ob = Instantiate(ExplosionPE, pos, Quaternion.identity);
            if (gameObject != null && gameObject.GetComponent<PhotonView>() != null)
                {
                DestroyGO();
                }
            //}



        }
    }
    public void DestroyGO()
    {
        obspv.RPC("DestroyAll",RpcTarget.All);
    }
    [PunRPC]
    public void DestroyAll()
    {
        Destroy(gameObject);
    }
}
