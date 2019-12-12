using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;   //singleton of class
    public GameObject offlineButton;
    public GameObject joinButton;
    public GameObject cancelButton;
    public GameObject trainingButton;

    Vector3 cornerposition = new Vector3(675, -285, 0);
    Vector3 centerposition = new Vector3(100, 0, 0);

    string regionCode = "usw";
    string gameVersion = "1";


    private void Awake()
    {
        lobby = this; //creates/initializes the singleton, lives within the Main menu scene

        PhotonNetwork.AutomaticallySyncScene = true;  //this enables use of PhotonNetwork.LoadLevel() on master client and all clients in the same room sync their level automatically
        DontDestroyOnLoad(lobby);
    }

    void Start()
    {
        //PhotonNetwork.ConnectToRegion(regionCode);
        //PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "";
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings(); //Connects to Master photon server.
        PhotonNetwork.LocalPlayer.SetScore(0);

    }



    public override void OnConnectedToMaster()      //callback function
    {
        Debug.Log("Player has connected to the Photon master server");
        offlineButton.SetActive(false);
        joinButton.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        cancelButton.transform.localPosition = centerposition;
    }


    public void OnBattleButtonClicked()
    {
        Debug.Log("Join Button was clicked");
        joinButton.SetActive(false);
        trainingButton.SetActive(false);
        //cancelButton.SetActive(true);
        PhotonNetwork.JoinRoom("bodyBuffet");
    }

    public void OnTrainingButtonClicked()
    {
        Debug.Log("Training Button was clicked");
        joinButton.SetActive(false);
        trainingButton.SetActive(false);
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.JoinRoom("training");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)    //callback function
    {
        Debug.Log("Tried to join game but failed. There was no open games available");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            Debug.Log("Welcome to the bodyBuffet room");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'shark-feed'");
                cancelButton.transform.localPosition = cornerposition;
                PhotonNetwork.LoadLevel("shark-feed");

            }
            else
            {
                LoadArena();
            }
        }
        else
        {
            Debug.Log("Welcome to the training room");
            Debug.Log("We load the 'training'");
            cancelButton.transform.localPosition = cornerposition;
            PhotonNetwork.LoadLevel("Training");
        }
        //LoadArena();
    }


    void LoadArena()
    {
        Debug.Log("Arena loaded");
        if (!PhotonNetwork.IsMasterClient)
        {
            //Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master client");
        }
        //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("shark-feed");
    }

    void CreateRoom()
    {

        //Debug.Log("Trying to create a new Room");
        if (!PhotonNetwork.OfflineMode)
        {
            string roomName = "bodyBuffet";
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomName, roomOps);
        }
        else
        {
            string roomName = "training";
            RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(roomName, roomOps);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)    //callback function
    {
        //Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        cancelButton.transform.localPosition = centerposition;
        CreateRoom();//retrys to create room
    }

    public override void OnCreatedRoom()
    {
        //Debug.Log("the bodyBuffet room has been created");
    }


    public void OnCancelButtonClicked()
    {
        //Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        //cancelButton.SetActive(true);
        joinButton.SetActive(true);
        trainingButton.SetActive(true);
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        cancelButton.transform.localPosition = centerposition;
        SceneManager.LoadScene(0);
    }

}
