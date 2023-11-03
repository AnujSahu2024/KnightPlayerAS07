using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class fireball : MonoBehaviourPunCallbacks
{
    private Rigidbody ballRG;
    private float fireForce;
    private PhotonView ballPhotonView;
    internal int throwPlayer;
    // Start is called before the first frame update
    void Start()
    {
        fireForce = 30f;
        Debug.Log("Ball start called "+ throwPlayer);
        ballRG = GetComponent<Rigidbody>();
        ballPhotonView = GetComponent<PhotonView>();
        //Debug.Log("<color=Green> Ball  " +" -> "+
        //    Movement.Instance.gameObject.GetComponent<PhotonView>().ViewID+ " Direction </color>");
        Vector3 direction_2 = transform.forward;
        ballPhotonView.RPC("Addforce", RpcTarget.All, fireForce, direction_2);
    }
    private void Update()
    {
        if(ballRG.velocity.magnitude<1f)
        {
            Destroy(gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
        if(!ballPhotonView.IsMine && ballRG.velocity.magnitude<0.01f)
        {
            Debug.LogError("Need To destroy");
            Destroy(gameObject);
        }
    }
    [PunRPC]
    public void Addforce(float fireforce,Vector3 direction)
    {
        ballRG = GetComponent<Rigidbody>();
        ballRG.AddForce(direction * fireForce, ForceMode.Impulse);
    }
    [PunRPC]
    public void UpdateScore(Dictionary<int,int> playerScore)
    {
        Debug.Log("<color=yellow>Update Score RPC called </color>");
        foreach (int vid in playerScore.Keys)
        {
            Debug.Log("<color=yellow>player Ids " + vid + " ->  score " + playerScore[vid]+"</color>");
        }
        OnlineMatchManager.Instance.setScore(playerScore);
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.transform.tag == "Person")
        {
            Debug.Log("<color=yellow>Exit Collide with person</color>");
        }
    }
    [PunRPC]
    public void TestRpc()
    {
        Debug.Log("<color=yellow>Test RPC called </color>");
       
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Person")
        {
            Debug.Log("collide with person");
            if (ballPhotonView.IsMine)
            {
                Debug.Log("<color=Red>fireball || collide with Person</color>");
                OnlineMatchManager.Instance.playerVIDdict[other.gameObject.GetComponent<PhotonView>().ViewID] -= 1;
                OnlineMatchManager.Instance.SetHealth(OnlineMatchManager.Instance.playerVIDdict);
                //Dictionary<int, int> PVidTemp = OnlineMatchManager.Instance.playerVIDdict;
                //ballPhotonView.RPC("UpdateScore", RpcTarget.All, PVidTemp);
                //Debug.LogError("<color=Yellow>Collide called - other id " + other.gameObject.GetComponent<PhotonView>().ViewID + "</color>");
                if (gameObject != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
               
                //return 0;
            }
        }
        if (other.gameObject.tag == "obsticals")
        {
            ShootSound.Instance.PlayShoot();
            Debug.Log("collide with obstical");
            if (ballPhotonView.IsMine)
            {
                if (this.throwPlayer > 0)
                {
                    OnlineMatchManager.Instance.playerScoredict[this.throwPlayer] += 1;
                    OnlineMatchManager.Instance.Setscorelocal();
                    if (gameObject != null)
                    {
                        //PhotonNetwork.Destroy(other.gameObject);
                        PhotonNetwork.Destroy(gameObject);
                        
                    }
                }
                
            }
        }
        if(other.gameObject.tag=="ground")
        {
            Debug.Log("collide with ground");
            if (ballPhotonView.IsMine)
            {
                if (gameObject != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }

            }
        }
        else
        {
            Debug.Log("collide with else");
            if (ballPhotonView.IsMine)
            {
                if (gameObject != null)
                {
                    PhotonNetwork.Destroy(gameObject);
                }

            }
        }
        if (ballPhotonView.IsMine)
        {
            if (gameObject != null)
            {
                Debug.Log("Collide  Detect");
                PhotonNetwork.Destroy(gameObject);
            }

        }
        else
        {
            Debug.LogError("Destroy Me =============>");
        }
    }
    
}
/*
 ///Garbage
 ///


---


/*private void OnCollisionEnter(Collision other)
    {
        //ballPhotonView.RPC("TestRpc",RpcTarget.All);
        Debug.Log("<color=Red> Player "+other.gameObject.name+"</color>");
        if(other.transform.tag=="Person")
        {
            
            //Debug.Log("<color=yellow>Collide with person</color>");
            //Debug.Log("<color=Yellow>Collide called - other id "+ other.gameObject.GetComponent<PhotonView>().ViewID +"</color>");
            //OnlineMatchManager.Instance.playerVIDdict[other.gameObject.GetComponent<PhotonView>().ViewID] -= 1;
            //Dictionary<int, int> PVidTemp = OnlineMatchManager.Instance.playerVIDdict;
            //ballPhotonView.RPC("UpdateScore", RpcTarget.All, PVidTemp);
            //Destroy(gameObject.GetComponent<fireball>(),1);
            //Destroy(gameObject);


        }
        else if(other.transform.tag== "obsticals")
        {
            Debug.LogErrorFormat("<color=Pink> Player {0} Throw the ball </color>",this.throwPlayer);
            if(this.throwPlayer>0)
            {
                OnlineMatchManager.Instance.playerScoredict[this.throwPlayer] += 1;
                
            }
            OnlineMatchManager.Instance.Setscorelocal();
            //this.GetComponent<Rigidbody>().velocity = 0f;
            Debug.Log("<color=yellow>Collide with obsticals</color>");
            //Destroy(this.gameObject.GetComponent<Rigidbody>());
            //Destroy(other.gameObject);
            //PhotonNetwork.Destroy(gameObject.GetComponent<fireball>());
            Destroy(gameObject);
            //PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject.GetComponent<fireball>());
            
        }
        else 
        {
            if (other.transform.tag == "ground")
            {
                Destroy(gameObject.GetComponent<fireball>(),1);
                Destroy(gameObject);
                Debug.Log("<color=yellow>Collide with Ground</color>");

            }
            

        }
    }



---
---
 //if (other.transform.tag == "Person")
        //{
        //    Movement.Instance.playerLife--;
        //    if(ballPhotonView.IsMine)
        //    {
        //        Debug.Log("Other id -> "+other.gameObject.GetComponent<PhotonView>().ViewID);
        //        if(OnlineMatchManager.Instance.playerViewId[0]== other.gameObject.GetComponent<PhotonView>().ViewID)
        //        {
        //            OnlineMatchManager.Instance.player2Score--;
        //        }
        //        if(OnlineMatchManager.Instance.playerViewId[1] == other.gameObject.GetComponent<PhotonView>().ViewID)
        //        {
        //            OnlineMatchManager.Instance.player1Score--;
        //        }
        //    }
        //    if(!ballPhotonView.IsMine)
        //    {
        //        Debug.Log("Its not mine id "+other.gameObject.GetComponent<PhotonView>().ViewID);
        //    }
        //    Debug.Log("Life " + Movement.Instance.playerLife);
        //   // Destroy(this.gameObject);
        //}
---------------
//ballRG.mass = 0;
            // OnlineMatchManager.Instance.UpdateScore();
            //ballRG.velocity = Vector3.zero;
------------------

  public void Go()
    {
       // ballRG.AddForce(new Vector3(0f,0f,1f) * fireForce, ForceMode.Impulse);
    }
--------
  // ballRG.mass = 0;
        //Go();
        //ApplyForce();
        // ballPhotonView.RPC("ApplyForceRPC", RpcTarget.All, fireForce);
        //Vector3 direction = Vector3.zero;
        /// if first player throw a ball then use the below direction

        //  direction = new Vector3(0f, 0f, 1f);

        /// if Second player throw a ball then use the below direction

        //  direction = new Vector3(0f, 0f, -1f);
        //direction = OnlineMatchManager.Instance.direction;
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        //{
        //    direction = new Vector3(0f, 0f, -1f);
        //}
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        //{
        //    direction = new Vector3(0f, 0f, -1f);
        //}
        //OnlineMatchManager.Instance.direction = Vector3.zero;
 */