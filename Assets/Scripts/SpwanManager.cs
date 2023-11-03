using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SpwanManager : MonoBehaviourPunCallbacks
{
    public static SpwanManager Instance;
    public Transform[] spwanPlayerTransform;
    public GameObject playerPrefab,cameraP1,cameraP2;
   // internal GameObject[] PTiles;
    private void Awake()
    {
        Debug.Log("SpawnManager Awake ");
        if (SpwanManager.Instance == null)
        {
            SpwanManager.Instance = this;
        }
    }
    void Start()
    {   
        //PTiles= GameObject.FindGameObjectsWithTag("Tile");
        //for(int i=0;i<PTiles.Length;i++)
        //{
        //    Debug.Log("Tile Name "+PTiles[i].name);
        //}
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer(0);
            cameraP1.SetActive(true);
            cameraP2.SetActive(false);
        }
        else
        {
            SpawnPlayer(1);
            cameraP1.SetActive(false);
            cameraP2.SetActive(true);
        }

    }
    void SpawnPlayer(int spawnPointIndex)
    {
        //Transform spawnPoint = spwanPlayerTransform[spawnPointIndex];
        Vector3 playerPosition= spwanPlayerTransform[spawnPointIndex].position;
        Vector3 pp = spwanPlayerTransform[spawnPointIndex].localPosition;
        Quaternion pr = spwanPlayerTransform[spawnPointIndex].localRotation;
        //Vector3 pp = PTiles[1].transform.localPosition;
        //Vector3 pp = new Vector3(PTiles[1].transform.localPosition.x, spwanPlayerTransform[spawnPointIndex].localPosition.y, spwanPlayerTransform[spawnPointIndex].localPosition.z);
        // Quaternion pr = PTiles[1].transform.localRotation;
        Quaternion playerRoatation = spwanPlayerTransform[spawnPointIndex].rotation;
        //PhotonNetwork.Instantiate(playerPrefab.name, playerPosition, playerRoatation);
        PhotonNetwork.Instantiate(playerPrefab.name, pp, pr);

    }
    // Update is called once per frame
   
}
