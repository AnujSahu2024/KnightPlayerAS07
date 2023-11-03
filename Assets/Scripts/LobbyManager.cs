using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomNameIF, joinRoomNameIF,playerNameIF;
    public static LobbyManager Instance;
    internal string playerName;
    public GameObject CreatePanel, JoinPanel,ErrorMessage;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnClickMakeRoomBtn()
    {   if(!string.IsNullOrEmpty(playerNameIF.text))
        {
            playerName = playerNameIF.text;
            CreatePanel.SetActive(true);
        }
        else
        {
            ErrorMessage.SetActive(true);
            ErrorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter Name...";
            StartCoroutine(Wait2Sec());
        }
        
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickJoinRoomBtn()
    {
        if (!string.IsNullOrEmpty(playerNameIF.text))
        {
            playerName = playerNameIF.text;
            JoinPanel.SetActive(true);
        }
        else
        {
            ErrorMessage.SetActive(true);
            ErrorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter Name...";
            StartCoroutine(Wait2Sec());
        }
        
    }
    public void OnCloseCreatePanel()
    {
        CreatePanel.SetActive(false);
    }
    public void OnCloseJoinPanel()
    {
        JoinPanel.SetActive(false);
    }
    public void MakeRoomBtnclick()
    {
        if(!string.IsNullOrEmpty(playerNameIF.text))
        {
            playerName = playerNameIF.text;
        }
        else
        {
            ErrorMessage.SetActive(true);
            ErrorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter Name...";
            StartCoroutine(Wait2Sec());
        }   
        
        if(!string.IsNullOrEmpty(createRoomNameIF.text) && !string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.NickName = playerName;
          bool xy=  PhotonNetwork.CreateRoom(createRoomNameIF.text);
            Debug.Log("Create Room "+xy);
        }
       

    }
    public IEnumerator Wait2Sec()
    {
        yield return new WaitForSeconds(2f);
        ErrorMessage.SetActive(false);
    }
    public void JoinRoomBtnclick()
    {
        if (!string.IsNullOrEmpty(playerNameIF.text))
        {
            playerName = playerNameIF.text;
        }
        else
        {
            ErrorMessage.SetActive(true);
            ErrorMessage.GetComponent<TextMeshProUGUI>().text = "Please Enter Name...";
            StartCoroutine(Wait2Sec());
        }
        if (!string.IsNullOrEmpty(joinRoomNameIF.text) && !string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.NickName = playerName;
            PhotonNetwork.JoinRoom(joinRoomNameIF.text);
        }
        
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount>2)
        {
            return;
        }
        print("<color=Red>On joinned Room</color>");
        PhotonNetwork.LoadLevel("GameD");
       // PhotonNetwork.LoadLevel("GameScene");

    }

}
