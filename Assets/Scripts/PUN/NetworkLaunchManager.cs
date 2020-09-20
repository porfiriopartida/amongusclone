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


public class NetworkLaunchManager : MonoBehaviourPunCallbacks
{
    public GameConfiguration GameConfiguration;
    private bool _lockJoin;
    public GameEvent OnConnectedEvent;
    public GameEvent RoomRefreshedEvent;
    public GameEvent JoinedRoomEvent;

    // public GameEvent ColorChanged;
    public CharacterColors CharacterColors;
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

    private void Start()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (var player in players)
        {
            ResetCharacter(player);
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
            PhotonNetwork.RaiseEvent(EventsConstants.NOTIFY_IMPOSTOR, selected, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    
    public override void OnJoinedRoom()
    {
        RefreshRoomStatus();
        JoinedRoomEvent.Raise();
        // SceneStateManager.Instance.SetColor(PhotonNetwork.LocalPlayer);
        SetCustomProperties(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        // SceneStateManager.Instance.AddPlayer(newPlayer);
        // SceneStateManager.Instance.SetColor(newPlayer);
        SetCustomProperties(newPlayer);
        RefreshRoomStatus();
    }

    private void ResetCharacter(Player player)
    {
        Hashtable hashtable = new Hashtable();
            
        hashtable[CustomProperties.IS_IMPOSTOR] = false;
        hashtable[CustomProperties.IS_ALIVE] = true;
            
        player.SetCustomProperties(hashtable);
    }

    private void SetCustomProperties(Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ResetCharacter(player);
            // SceneStateManager.Instance.SetColor(PhotonNetwork.LocalPlayer, player.ActorNumber);
            
            List<int> takenColors = SceneStateManager.Instance.GetTakenColors();
            int colorsCount = CharacterColors.colors.Count;
            for (int i = 0; i < colorsCount; i++)
            {
                if (!takenColors.Contains(i))
                {
                    Debug.Log("Giving color " + i + "to " + player);
                    SceneStateManager.Instance.SetColor(player, i);
                    break;
                }
            }
        }
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
    }

    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     Debug.Log("Loading new properties: " + propertiesThatChanged);
    // }

    #endregion
}
