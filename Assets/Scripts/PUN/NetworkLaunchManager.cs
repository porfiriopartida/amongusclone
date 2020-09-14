using System;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Utils;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;


public class NetworkLaunchManager : MonoBehaviourPunCallbacks
{
    public GameConfiguration GameConfiguration;
    private bool _lockJoin;
    public GameEvent OnConnectedEvent;
    public GameEvent RoomRefreshedEvent;
    public GameEvent JoinedRoomEvent;

    private void RefreshRoomStatus()
    {
        RoomRefreshedEvent.Raise();
    }
    void Awake()
    {
        //All clients sync their levels to the master user.
        PhotonNetwork.AutomaticallySyncScene = true;
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
    }

    #region Photon Callbacks 
    public void ConnectToPhotonServer() {
        if (String.IsNullOrEmpty(PhotonNetwork.NickName.Trim()))
        {
            return;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            //Trigger event here: Connected to server
            OnConnectedEvent.Raise();
        }
        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " connected to Photon Server");
        
        JoinRandomRoom();
    }
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateAndJoinRoom();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// This method is only executed by the authoritative user.
    /// </summary>
    public void StartGame()
    {
        _lockJoin = true;
        
        if (PhotonNetwork.IsMasterClient)
        {
            var customProps = SceneStateManager.Instance.ParseProperties();
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);
            
            string[] selected = AmongosRandomGenerator.GenerateImpostors(PhotonNetwork.PlayerList, (int) customProps["TotalImpostors"]);
            #if UNITY_EDITOR
            if (GameConfiguration.IsDebug)
            {
                List<string> impostors = new List<string>();
                foreach (var gameConfigurationDebugImpostor in GameConfiguration.DebugImpostors)
                {
                    impostors.Add(PhotonNetwork.PlayerList[gameConfigurationDebugImpostor].UserId);
                }
                selected = impostors.ToArray();
            }
            #endif
            
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.NotifyImpostor, selected, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    
    public override void OnJoinedRoom()
    {
        Player[] playersList = PhotonNetwork.PlayerList;
        SceneStateManager.Instance.Clear();
        foreach (var player in playersList)
        {
            SceneStateManager.Instance.AddPlayer(player);
        }
        RefreshRoomStatus();
        
        JoinedRoomEvent.Raise();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        SceneStateManager.Instance.AddPlayer(newPlayer);
        RefreshRoomStatus();
    }
    #endregion

    #region PrivateMethods
    private void CreateAndJoinRoom() {
        string randomRoomName = "SAQUENALDANTON229"; // + Random.Range(0, 10000)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible= true;
        roomOptions.MaxPlayers = 10;
        roomOptions.PublishUserId = true;
        roomOptions.PlayerTtl = 3000;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Bad States can happen.
        PhotonNetwork.Disconnect();
        
        SceneManager.LoadScene("Scenes/Login");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RefreshRoomStatus();
        SceneStateManager.Instance.RemovePlayer(otherPlayer);
    }

    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     Debug.Log("Loading new properties: " + propertiesThatChanged);
    // }

    #endregion
}
