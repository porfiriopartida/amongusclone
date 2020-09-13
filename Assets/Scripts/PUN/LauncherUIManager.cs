using System.Collections;
using LopapaGames.Common.Core;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LauncherUIManager : MonoBehaviour
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;
    public GameObject OwnerSettings;
    public GameObject YouAreCrewmateSign;
    public GameObject YouAreImpostorSign;

    public GameEvent GameStartRequestedEvent;

    public GameConfiguration GameConfiguration;
    public GameObject ConfigurationUI;
    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        OwnerSettings.SetActive(false);
        YouAreCrewmateSign.SetActive(false);
        YouAreImpostorSign.SetActive(false);
        CloseConfiguration();
        if (PhotonNetwork.IsConnected)
        {
            EnterGamePanel.SetActive(false);
            LobbyPanel.SetActive(true);
            OnRefreshRoomStatus();
        }
    }


    public void OnShowCrewmateSign()
    {
        EnterGamePanel.SetActive(false);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        OwnerSettings.SetActive(false);
        YouAreImpostorSign.SetActive(false);

        YouAreCrewmateSign.SetActive(true);
        
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameScene());
        }
    }

    public void OnShowImpostorSign()
    {
        EnterGamePanel.SetActive(false);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        OwnerSettings.SetActive(false);
        YouAreCrewmateSign.SetActive(false);

        YouAreImpostorSign.SetActive(true);
        
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(GameScene());
        }
    }


    public void OnRefreshRoomStatus()
    {
        // Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        LobbyController lobbyController = LobbyPanel.GetComponent<LobbyController>();
        lobbyController.SetRoomTitle("Room:\n " + PhotonNetwork.CurrentRoom.Name + "\n\nPlayers: " +
                                     PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                                     PhotonNetwork.CurrentRoom.MaxPlayers + "\n\nMaster: " +
                                     PhotonNetwork.MasterClient.NickName);
        lobbyController.RenderPlayers();

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            OwnerSettings.SetActive(true);
        }
    }

    public void OnConnectedToServer()
    {
        ConnectionStatusPanel.SetActive(true);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    public void OnJoinedRoom()
    {
        LobbyPanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
    }

    public void GameStartPressed()
    {
        GameStartRequestedEvent.Raise();
    }

    public void OpenConfiguration()
    {
        ConfigurationUI.SetActive(true);
    }
    public void CloseConfiguration()
    {
        ConfigurationUI.SetActive(false);
    }

    public IEnumerator GameScene()
    {
        #if UNITY_EDITOR
        yield return new WaitForSeconds(GameConfiguration.ChangeSceneWaitingDebug);
        #else
        yield return new WaitForSeconds(GameConfiguration.ChangeSceneWaiting);
        #endif
        SceneManager.LoadScene("Scenes/GameScene");
    }
}