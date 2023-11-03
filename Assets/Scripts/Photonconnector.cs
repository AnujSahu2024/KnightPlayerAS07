using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class Photonconnector : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("connected to server ");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        print("On Joined Lobby Called ");
        base.OnJoinedLobby();
        SceneManager.LoadScene("Lobby");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from Server  reason " + cause.ToString());

    }
}
