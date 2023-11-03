using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPunCallbacks
{
    public static Movement Instance;
    private Rigidbody playerRB;
    public float jumpForce = 5.0f;
    public float moveSpeed = 3.0f;
    private Vector3 moveRight, moveLeft;
    private int currentTile = 1;
  //  private bool isJumping = false;
    internal int playerLife = 20;
    public GameObject fireBall, obsticals;
    private Vector3 rTile, mTile, lTile;
    internal Vector3 direction = Vector3.zero;
    private fireball fb;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
        //this.currentTile = 1;
        //if(PhotonNetwork.LocalPlayer.ActorNumber==1)
        //{
        //    rTile = SpwanManager.Instance.PTiles[0].transform.localPosition;
        //    mTile = SpwanManager.Instance.PTiles[1].transform.localPosition;
        //    lTile = SpwanManager.Instance.PTiles[2].transform.localPosition;
        //}
                
       
        Debug.LogError("player start called ");
        playerRB = GetComponent<Rigidbody>();
        moveRight = new Vector3(1f,0f,0f);
        moveLeft = new Vector3(-1f, 0f, 0f);
        OnlineMatchManager.Instance.playerViewId.Add(gameObject.GetComponent<PhotonView>().ViewID);
        OnlineMatchManager.Instance.playerVIDdict.Add(gameObject.GetComponent<PhotonView>().ViewID,3);
        OnlineMatchManager.Instance.playerScoredict.Add(gameObject.GetComponent<PhotonView>().ViewID, 0);
        if(OnlineMatchManager.Instance.playerViewId.Count==2 && PhotonNetwork.CurrentRoom.PlayerCount==2)
        {
            OnlineMatchManager.Instance.resetlistid();
        }
        if (OnlineMatchManager.Instance.playerViewId.Count == 3)
        {
            OnlineMatchManager.Instance.playerViewId.RemoveAt(2);
        }

    }
    public void PlayerDisconnect()
    {
        if(this.gameObject.GetComponent<PhotonView>().IsMine)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LeaveRoom();
        }
    }
   
    public void GetSpawnPositionobs(List<int> PosX,List<int> PosZ)
    {
        Debug.Log("GetSpanned Callled ");
        for (int i = 0; i < PosX.Count; i++)
        {
            Vector3 pp = new Vector3((float)PosX[i], 2.2f, (float)PosZ[i]);
            GameObject obs = PhotonNetwork.Instantiate(obsticals.name, pp, Quaternion.identity);
            obs.GetComponent<PhotonView>().TransferOwnership(null);
        }
        // this.gameObject.GetComponent<PhotonView>().RPC("SendSpawnPos", RpcTarget.All, PosX,PosZ);
    }
    [PunRPC]
    public void SendSpawnPos(List<int> PosX, List<int> PosZ)
    {
        OnlineMatchManager.Instance.SetObsticals( PosX,  PosZ);
    }
    public void SendPlayerscore(Dictionary<int, int> playerScore)
    {
        Debug.Log("<color=yellow>Update Score RPC called </color>");
        foreach (int vid in playerScore.Keys)
        {
            Debug.Log("<color=yellow>player Ids " + vid + " ->  score " + playerScore[vid] + "</color>");
        }
        Dictionary<int, int> temp=playerScore;
        this.gameObject.GetComponent<PhotonView>().RPC("sharePlayerdatascore", RpcTarget.All, temp);
    }
    [PunRPC]
    public void sharePlayerdatascore(Dictionary<int, int> playerScore)
    {
        Debug.Log("<color=yellow>sharePlayerdatascore Score RPC called </color>");
        foreach (int vid in playerScore.Keys)
        {
            Debug.Log("<color=yellow>player Ids " + vid + " ->  score " + playerScore[vid] + "</color>");
        }
        OnlineMatchManager.Instance.Setboard(playerScore);
    }
    public void SendPid(int[] Playerid)
    {

        this.gameObject.GetComponent<PhotonView>().RPC("UpdatePid", RpcTarget.Others, Playerid);
    }

    [PunRPC]
    public void UpdatePid(int[] uppid)
    {
        Debug.Log("Update uppid in Rpc other ");
        for (int i = 0; i < uppid.Length; i++)
        {
            Debug.Log(" P id s " + uppid[i]);

        }
       OnlineMatchManager.Instance.Getids(uppid);
    }
    public void GetUpdatedHelth(Dictionary<int, int> playerUpdatedHealth)
    {
        foreach (int key in playerUpdatedHealth.Keys)
        {
            Debug.LogFormat("<color=Yellow>Pkey {0} PScore {1}</color>", key, playerUpdatedHealth[key]);
        }
        Dictionary<int, int> temp = playerUpdatedHealth;
        this.gameObject.GetComponent<PhotonView>().RPC("SharePlayerHealth", RpcTarget.All, temp);
    }
    [PunRPC]
    public void SharePlayerHealth(Dictionary<int, int> playerUpdatedHealth)
    {
        OnlineMatchManager.Instance.setScore(playerUpdatedHealth);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(photonView.IsMine)
        {
            if (other.transform.tag == "ball")
            {
                Debug.Log("<color=Red>Movement || collide with ball</color>");
                Debug.Log("<color=Red></color>");
               // OnlineMatchManager.Instance.playerVIDdict[gameObject.GetComponent<PhotonView>().ViewID] -= 1;
               // OnlineMatchManager.Instance.SetHealth(OnlineMatchManager.Instance.playerVIDdict);

                //Dictionary<int, int> PVidTemp = OnlineMatchManager.Instance.playerVIDdict;
                //OnlineMatchManager.Instance.setScore(PVidTemp);
                //this.gameObject.GetComponent<PhotonView>().RPC("UpdateScore", RpcTarget.Others, PVidTemp);
                //Destroy(other.gameObject.GetComponent<fireball>(), 1);
                //Destroy(other.gameObject);

            }
        }
        
    }
    [PunRPC]
    public void UpdateScore(Dictionary<int, int> playerScore)
    {
        //Debug.Log("<color=yellow>Update Score RPC called </color>");
        foreach (int vid in playerScore.Keys)
        {
           // Debug.Log("<color=yellow>player Ids " + vid + " ->  score " + playerScore[vid] + "</color>");
        }
        OnlineMatchManager.Instance.setScore(playerScore);
    }
    public void SetIDList(List<int> Pid)
    {
        this.gameObject.GetComponent<PhotonView>().RPC("UpdateIdList", RpcTarget.Others, Pid);
    }
    [PunRPC]
    public void UpdateIdList(List<int> playerIDList)
    {
        OnlineMatchManager.Instance.setPlayerId(playerIDList);
    }
    public void SetScoreData(Dictionary<int, int>Psc)
    {
        Debug.Log("SetCore called ");
        foreach (int key in Psc.Keys)
        {
            //Debug.LogFormat("<color=Red>Keys {0} value {1}</color>", key, Psc[key]);
        }
        this.gameObject.GetComponent<PhotonView>().RPC("UpdateScoredict", RpcTarget.Others, Psc);
    }
    [PunRPC]
    public void UpdateScoredict(Dictionary<int, int> playerDataDict)
    {
       // Debug.Log("update Score dict Rpc callled ");
        foreach (int key in playerDataDict.Keys)
        {
            //Debug.LogFormat("<color=Red>Keys {0} value {1}</color>", key, playerDataDict[key]);
        }
        OnlineMatchManager.Instance.SetScoreDict(playerDataDict);
    }
    public void SetDictData(Dictionary<int ,int> PlayerVIdDic)
    {
        this.gameObject.GetComponent<PhotonView>().RPC("UpdateDict", RpcTarget.Others, PlayerVIdDic);
    }
    [PunRPC]
    public void UpdateDict(Dictionary<int, int> playerData)
    {
        Debug.Log("<color=yellow>Update Dict RPC called </color>");
        foreach (int vid in playerData.Keys)
        {
            //Debug.Log("<color=yellow>player Ids " + vid + " ->  score " + playerData[vid] + "</color>");

        }
        OnlineMatchManager.Instance.setPlayerData(playerData);
    }

    public void PMoveRight()
    {
        //if()
        //{

        //}
    }
    public void MovementPlayer()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
           // PMoveRight();
            transform.Translate(moveRight * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(moveLeft * moveSpeed * Time.deltaTime);
        }
        //if(Input.GetKey(KeyCode.Space))
        //{
        //    if(!Instance.isJumping)
        //    {
        //        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    }
        //}
    }
    public void Fireball()
    {
        Quaternion playerRotation;
        Vector3 inst;

        if(OnlineMatchManager.Instance.playerId==2)
        {
            //Debug.Log("<color=Green>Player 2 called</color>");
           playerRotation = Quaternion.Euler(transform.localRotation.x,180f, transform.localRotation.z);
            inst = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1.5f);
            direction = new Vector3(0f, 0f, -1f);
        }
        else
        {
            Debug.Log("<color=Green>Player 1 called</color>");
            inst = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1.5f);
            playerRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
            direction = new Vector3(0f, 0f, 1f);
        }

        //this.gameObject.GetComponent<PhotonView>().RPC("SetDirection",RpcTarget.All,direction);
        fb=PhotonNetwork.Instantiate(fireBall.name,inst, playerRotation).GetComponent<fireball>();
        //fb.throwPlayer=
    }
    [PunRPC]
    public void SetDirection(Vector3 direction)
    {
       // OnlineMatchManager.Instance.direction = direction;
    }
    public void TestFireball()
    {
        if(OnlineMatchManager.Instance.playerId == 2)
        {
           // Debug.Log("<color=Green>"+gameObject.GetComponent<PhotonView>().ViewID+" called </color>");
        }

        if (OnlineMatchManager.Instance.playerId == 1)
        {
            //Debug.Log("<color=Green>" + gameObject.GetComponent<PhotonView>().ViewID + " called </color>");
        }
        Quaternion playerRotation;
        Vector3 inst;

        if (OnlineMatchManager.Instance.playerId == 2)
        {
            Debug.Log("<color=Green>Player 2 called  "+transform.rotation+"</color>");
            playerRotation = Quaternion.Euler(transform.localRotation.x, 180f, transform.localRotation.z);
            inst = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1.5f);
            direction = new Vector3(0f, 0f, -1f);
        }
        else
        {
            Debug.Log("<color=Green>Player 1 called  " + transform.rotation + "</color>");
            inst = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1.5f);
            playerRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
            direction = new Vector3(0f, 0f, 1f);
        }

        //this.gameObject.GetComponent<PhotonView>().RPC("SetDirection",RpcTarget.All,direction);
        fb=PhotonNetwork.Instantiate(fireBall.name, inst, playerRotation).GetComponent<fireball>();
        fb.throwPlayer = gameObject.GetComponent<PhotonView>().ViewID;
        fb.GetComponent<PhotonView>().TransferOwnership(null);
        BallThrower();
    }
    public void BallThrower()
    {
       gameObject.GetComponent<PhotonView>().RPC("OnBallThrown", RpcTarget.All, this.gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
    }
    [PunRPC]
    private void OnBallThrown(int playerid)
    {
       // Debug.Log("<color=Yellow> PVID Called "+gameObject.GetComponent<PhotonView>().ViewID+"</color>");
      //  Debug.Log("<color=Green>Player called  " + playerid + " called </color>");
       // OnlineMatchManager.Instance.CalculateScore(playerid);
        
    }
    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            MovementPlayer();
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("<color=Green> Player  " + this.gameObject.GetComponent<PhotonView>().ViewID +" Throw ball</color>");
                // Fireball();
                TestFireball();
               


            }
        }
       
    }
  
}
