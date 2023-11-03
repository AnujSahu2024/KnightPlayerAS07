using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public enum MatchStatus
{
    Won,
    Lose,
    Draw
}
public class OnlineMatchManager : MonoBehaviourPunCallbacks
{
    public static OnlineMatchManager Instance;
    public GameObject GameStatus,won,lose,draw;
    public bool isMatchEnd = false;
    public GameObject WaitingPanel, GamePanel;
    public MatchStatus p1matchStatus,p2matchStatus;
    public TextMeshProUGUI playerName, playerScore,playerlife,oponentlife, oponentName, oponentScore;
    internal int playerId;
    internal List<int> playerViewId = new List<int>();
    internal Dictionary<int, int> playerVIDdict = new Dictionary<int, int>();
    internal Dictionary<int, int> playerScoredict = new Dictionary<int, int>();
    internal Dictionary<int, string> playerNamedict = new Dictionary<int, string>();
    internal int player1Life = 3, player2Life = 3, player1Score = 0, player2Score = 0;
    private bool hasjoined = false, isReset=false;
    PhotonView onlineMatchPV;
    public List<int> pid2 = new List<int>();
    public int Pcount = 0;
    public int ballcount = 0;
    public bool noball;
    //internal Vector3 direction = Vector3.zero;

    public void clearData()
    {
        playerViewId.Clear();
        playerVIDdict.Clear();
        playerScoredict.Clear();
        playerNamedict.Clear();
        pid2.Clear();
    }
    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    // Start is called before the first frame update
    private void Awake()
    {
        noball = false;
        //DontDestroyOnLoad(gameObject);
        clearData();
        onlineMatchPV = GetComponent<PhotonView>();
        Debug.Log("onlinemanager Awake called ");
        if(Instance==null)
        {
            Instance = this;
        }
        Debug.Log("Ac num -> " + PhotonNetwork.LocalPlayer.ActorNumber);
        OnlineMatchManager.Instance.playerId=PhotonNetwork.LocalPlayer.ActorNumber;

    }
    private void Start()
    {
        isMatchEnd = false;
        /// Debug.Log("player VID "+P);
        
    }
    public void SetHealth(Dictionary<int, int> playerHealth)
    {
        foreach (int key in playerHealth.Keys)
        {
            Debug.LogFormat("<color=Pink>Pkey {0} PScore {1}</color>", key, playerHealth[key]);
        }
        Movement.Instance.GetUpdatedHelth(OnlineMatchManager.Instance.playerVIDdict);
    }
    public void Updatescorel()
    {
        foreach(int key in OnlineMatchManager.Instance.playerScoredict.Keys)
        {
            Debug.LogFormat("<color=Pink>Pkey {0} PScore {1}</color>",key,OnlineMatchManager.Instance.playerScoredict[key]);
        }
    }


    public void SetObsticals(List<int> PosX, List<int> PosZ)
    {

    }
    public void Setscorelocal()
    {
        SendPlayerscoreother();
        //Debug.Log("Set score local called ");
        //foreach (int key in OnlineMatchManager.Instance.playerScoredict.Keys)
        //{
        //    Debug.LogFormat("<color=Red>Keys {0} value {1}</color>", key, OnlineMatchManager.Instance.playerScoredict[key]);
        //}
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        //{
        //    player1Score = OnlineMatchManager.Instance.playerScoredict[OnlineMatchManager.Instance.playerViewId[1]];
        //    player2Score = OnlineMatchManager.Instance.playerScoredict[OnlineMatchManager.Instance.playerViewId[0]];
        //}
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        //{
        //    player1Score = OnlineMatchManager.Instance.playerScoredict[OnlineMatchManager.Instance.playerViewId[1]];
        //    player2Score = OnlineMatchManager.Instance.playerScoredict[OnlineMatchManager.Instance.playerViewId[0]];
        //}
        //SetScore();

    }
   public void SendPlayerscoreother()
    {
        Movement.Instance.SendPlayerscore(OnlineMatchManager.Instance.playerScoredict);
    }
     public void Setboard(Dictionary<int, int> playerScore)
    {
        OnlineMatchManager.Instance.ballcount++;
        Debug.LogError("Ball Destroy =>  "+ OnlineMatchManager.Instance.ballcount);
        if (OnlineMatchManager.Instance.ballcount==5)
        {
            OnlineMatchManager.Instance.noball = true;
        }
        Debug.LogError("Get Data from oponent ");
        //OnlineMatchManager.Instance.playerScoredict.Clear();
        OnlineMatchManager.Instance.playerScoredict = playerScore;
        foreach (int key in playerScore.Keys)
        {
            Debug.LogFormat("<color=Red>Keys {0} value {1}</color>", key, OnlineMatchManager.Instance.playerScoredict[key]);
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            player1Score = playerScore[OnlineMatchManager.Instance.playerViewId[1]];
            player2Score = playerScore[OnlineMatchManager.Instance.playerViewId[0]];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            player1Score = playerScore[OnlineMatchManager.Instance.playerViewId[1]];
            player2Score = playerScore[OnlineMatchManager.Instance.playerViewId[0]];
        }
        SetScore();
    }
   
        public void SetScore()
    {
       
        
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            playerScore.text = player1Score.ToString();
            oponentScore.text = player2Score.ToString();
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            playerScore.text = player2Score.ToString();
            oponentScore.text = player1Score.ToString();
        }

       if (OnlineMatchManager.Instance.noball)
        {
            CheckWinloseforend();
        }
       
    }
    public void CheckWinloseforend()
    {
        Debug.LogError("Player 1 Score => "+player1Score);
        Debug.LogError("Player 2 Score => " + player2Score);

        if (player1Score < player2Score)
        {
            p1matchStatus = MatchStatus.Lose;
            p2matchStatus = MatchStatus.Won;
        }
        if (player2Score < player1Score)
        {
            p2matchStatus = MatchStatus.Lose;
            p1matchStatus = MatchStatus.Won;
        }
        if (player1Score == player2Score)
        {
            p1matchStatus = MatchStatus.Draw;
            p2matchStatus = MatchStatus.Draw;
        }

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            Debug.LogError("AC 1   ");
            Debug.LogError("P1Match status" + p1matchStatus);
            Debug.LogError("P2Match status" + p2matchStatus);
            GameStatus.SetActive(true);
            if (p1matchStatus == MatchStatus.Won)
            {
                won.SetActive(true);
            }
            if (p1matchStatus == MatchStatus.Lose)
            {
                lose.SetActive(true);
            }
            if (p1matchStatus == MatchStatus.Draw)
            {
                draw.SetActive(true);
            }
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            Debug.LogError("AC 2");
            Debug.LogError("P1Match status" + p1matchStatus);
            Debug.LogError("P2Match status" + p2matchStatus);
            GameStatus.SetActive(true);
            if (p2matchStatus == MatchStatus.Won)
            {
                won.SetActive(true);
            }
            if (p2matchStatus == MatchStatus.Lose)
            {
                lose.SetActive(true);
            }
            if (p2matchStatus == MatchStatus.Draw)
            {
                draw.SetActive(true);
            }
        }
    }
    public void setPlayerData(Dictionary<int, int> PlayerVIdData)
    {
        if(OnlineMatchManager.Instance.playerVIDdict.Count!=2)
        {
            OnlineMatchManager.Instance.playerVIDdict.Clear();
            foreach (int key in PlayerVIdData.Keys)
            {
                OnlineMatchManager.Instance.playerVIDdict.Add(key, PlayerVIdData[key]);
            }
            foreach(int key in PlayerVIdData.Keys)
            {
                OnlineMatchManager.Instance.playerScoredict.Add(key,0);
            }
           
        }
        
    }
    public void GameBoardData()
    {
        Debug.Log("<color=Green> Game Board Data </color>");
        Debug.Log("<color=Green>Player Vid 0 -> " + playerViewId[0] + "</color>");
        Debug.Log("<color=Green>Player Vid 1 -> " + playerViewId[1] + "</color>");
        //foreach (int key in OnlineMatchManager.Instance.playerVIDdict.Keys)
        //{
        //    Debug.Log("<color=Green> Player id -> "+key+" value -> "+OnlineMatchManager.Instance.playerVIDdict[key]+"</color>");
        //}
    }
    public void GID()
    {
        Debug.Log("<color=Red>GID called </color>");
        Debug.Log("Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        for(int i=0;i< PhotonNetwork.CurrentRoom.PlayerCount;i++)
        {
            Debug.Log("Player name "+PhotonNetwork.PlayerList[i].NickName);
        }
        if(PhotonNetwork.PlayerList.Length==2)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                playerName.text = PhotonNetwork.PlayerList[0].NickName;
                playerlife.text = player1Life.ToString();
                oponentName.text = PhotonNetwork.PlayerList[1].NickName;
                oponentlife.text = player2Life.ToString();
            }
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                playerName.text = PhotonNetwork.PlayerList[1].NickName;
                playerlife.text = player2Life.ToString();
                oponentName.text = PhotonNetwork.PlayerList[0].NickName;
                oponentlife.text = player1Life.ToString();
            }
        }

        for(int i=0;i<playerViewId.Count;i++)
        {
            Debug.Log("View Ids " + playerViewId[i]);
        }
        foreach (int vid in playerVIDdict.Keys)
        {
            Debug.Log("<color=yellow>PlayerView Ids " + vid + " -> " + playerVIDdict[vid]+"</color>");
           //
        }
        if(playerVIDdict.Count==2)
        {
            Movement.Instance.SetDictData(playerVIDdict);
        }

    }
    public void SetScoreDict(Dictionary<int, int> scoreBoard)
    {
        Debug.Log("Set Score Dict in"+ OnlineMatchManager.Instance.playerScoredict.Count);
        foreach(int key in scoreBoard.Keys)
        {
            Debug.LogFormat("<color=Red>Keys {0} value {1}</color>",key, scoreBoard[key]);
        }
        if (OnlineMatchManager.Instance.playerScoredict.Count == 2)
        {
            OnlineMatchManager.Instance.playerScoredict.Clear();
            foreach(int key in scoreBoard.Keys)
            {
                OnlineMatchManager.Instance.playerScoredict.Add(key,scoreBoard[key]);
            }
        }
    }
    //Set Life and it get by rpc
    public void setScore(Dictionary<int, int> scoreBoard)
    {
        Debug.Log("<color=Green>Player Vid 0 -> "+ playerViewId[0]+"</color>");
        Debug.Log("<color=Green>Player Vid 1 -> " + playerViewId[1]+"</color>");
        OnlineMatchManager.Instance.playerVIDdict = scoreBoard;
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            player1Life = scoreBoard[playerViewId[1]];
            player2Life = scoreBoard[playerViewId[0]];
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            player1Life = scoreBoard[playerViewId[1]];
            player2Life = scoreBoard[playerViewId[0]];
        }

       
        UpdateScore();
    }
    public void UpdateScore()
    {
        Debug.Log("ViD P1 => "+playerViewId[0]);
        Debug.Log("ViD P2 => "+playerViewId[1]);
        foreach (int vid in playerVIDdict.Keys)
        {
            Debug.Log("player Ids " + vid + " ->  score " + playerVIDdict[vid]);
            //
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            playerlife.text = player1Life.ToString();
            oponentlife.text = player2Life.ToString();
        }
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            playerlife.text = player2Life.ToString();
            oponentlife.text = player1Life.ToString();
        }
        if(player2Life==0 || player1Life==0 )
        {
            Debug.LogError("Player 1"+player1Life);
            Debug.LogError("Player 2" + player2Life);
            isMatchEnd = true;
        }
        //stat
        SetStatus();
    }
    public void SetStatus()
    {
        if (isMatchEnd)
        {
            if (player1Life == 0)
            {
                p1matchStatus = MatchStatus.Lose;
                p2matchStatus = MatchStatus.Won;
            }
            if (player2Life == 0)
            {
                p2matchStatus = MatchStatus.Lose;
                p1matchStatus = MatchStatus.Won;
            }
            if (player1Life == 0 && player2Life == 0)
            {
                p1matchStatus = MatchStatus.Draw;
                p2matchStatus = MatchStatus.Draw;
            }
            Debug.LogError("P1Match status"+p1matchStatus);
            Debug.LogError("P2Match status" + p2matchStatus);
            if (player1Life == 0 || player2Life == 0)
            {
               // Movement.Instance.PlayerDisconnect();
                if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                {
                    Debug.LogError("AC 1   ");
                    Debug.LogError("P1Match status" + p1matchStatus);
                    Debug.LogError("P2Match status" + p2matchStatus);
                    GameStatus.SetActive(true);
                    if (p1matchStatus == MatchStatus.Won)
                    {
                        won.SetActive(true);
                    }
                    if (p1matchStatus == MatchStatus.Lose)
                    {
                        lose.SetActive(true);
                    }
                    if (p1matchStatus == MatchStatus.Draw)
                    {
                        draw.SetActive(true);
                    }
                }
                if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                {
                    Debug.LogError("AC 2");
                    Debug.LogError("P1Match status" + p1matchStatus);
                    Debug.LogError("P2Match status" + p2matchStatus);
                    GameStatus.SetActive(true);
                    if (p2matchStatus == MatchStatus.Won)
                    {
                        won.SetActive(true);
                    }
                    if (p2matchStatus == MatchStatus.Lose)
                    {
                        lose.SetActive(true);
                    }
                    if (p2matchStatus == MatchStatus.Draw)
                    {
                        draw.SetActive(true);
                    }
                }
                
                // Movement.Instance.PlayerDisconnect();
            }
        }

    }
    private void PVID()
    {
        if(OnlineMatchManager.Instance.playerViewId.Count==2)
        {
            if(OnlineMatchManager.Instance.playerVIDdict.Count==2)
            { OnlineMatchManager.Instance.playerViewId.Clear();
                foreach(int key in OnlineMatchManager.Instance.playerVIDdict.Keys)
                {
                    OnlineMatchManager.Instance.playerViewId.Add(key);
                }
            }
            
        }
    }
    public void setPlayerId(List<int>Pid)
    {
        OnlineMatchManager.Instance.playerViewId.Clear();
        for(int i=0;i<Pid.Count;i++)
        {
            OnlineMatchManager.Instance.playerViewId.Add(Pid[i]);
        }
        
                
    }
      public void PidL()
    {
        Debug.Log("called Pid");
        //foreach(int key in playerScoredict.Keys)
        //{
        //    Debug.LogFormat("<color=Green> PScore id {0} score {1} </color>",key,playerScoredict[key]);
        //}
        for (int i = 0; i < OnlineMatchManager.Instance.playerViewId.Count; i++)
        {
             Debug.Log("<color=Green> PId" + OnlineMatchManager.Instance.playerViewId[i] + "</color>");
            //Debug.Log("<color=Green> PScore" + OnlineMatchManager.Instance.playerScoredict[i] + "</color>");
        }
    }
    public void Ps()
    {
        Debug.Log("Ps called "+ OnlineMatchManager.Instance.playerVIDdict.Count+ PhotonNetwork.CountOfPlayersInRooms);
        foreach (int key in playerScoredict.Keys)
        {
            Debug.LogFormat("<color=Red>Keys {0} value {1}</color>", key,playerScoredict[key]);
        }
        if (OnlineMatchManager.Instance.playerVIDdict.Count == 2)
        {
            Movement.Instance.SetScoreData(OnlineMatchManager.Instance.playerScoredict);
        }

    }
   
    public void Getids(int[] pid)
    {
        for(int i=0;i<OnlineMatchManager.Instance.playerViewId.Count;i++)
        {
            Debug.Log("X : "+OnlineMatchManager.Instance.playerViewId[i]);
        }
        OnlineMatchManager.Instance.playerViewId.Clear();
        for (int i = 0; i < pid.Length; i++)
        {
            Debug.Log("pid " + pid[i]);
            OnlineMatchManager.Instance.playerViewId.Add(pid[i]);
        }
        
    }
    public void resetlistid()
    {
       
        Debug.LogError("Reset list called ");
        foreach (int key in OnlineMatchManager.Instance.playerVIDdict.Keys)
        {
            Debug.Log("Key"+key);
            pid2.Add(key);
            //OnlineMatchManager.Instance.playerViewId.Add(key);
        }
        if(OnlineMatchManager.Instance.playerVIDdict.Count==2)
        {
            if(pid2.Count==2)
            {
                Movement.Instance.SendPid(pid2.ToArray());
            }
            
        }
    }
    public void UIManipulate()
    {
        WaitingPanel.SetActive(false);
        GamePanel.SetActive(true);
    }
    private void Update()
    {
        if(!hasjoined && PhotonNetwork.PlayerList.Length == 2)
        {
            UIManipulate();
            if(ObsticalManager.Instance!=null)
            {
                Movement.Instance.GetSpawnPositionobs(ObsticalManager.Instance.listPosX, ObsticalManager.Instance.listPosZ);
            }
           
            GID();
            PVID();
            Ps();
            hasjoined = true;
            
        }
        if(OnlineMatchManager.Instance.Pcount==2 && !isReset)
        {
            //resetlistid();
            isReset = true;
        }
        //if(OnlineMatchManager.Instance.playerViewId.Count==2)
        //{

        //}
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PidL();
            //GameBoardData();
        }
    }
}
