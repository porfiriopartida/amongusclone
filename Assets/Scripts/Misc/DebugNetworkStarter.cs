using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PUN;
using UnityEngine;

public class DebugNetworkStarter : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private NetworkLaunchManager _networkLaunchManager;
    public bool IsImpostor;
    public GameSceneManager _gameSceneManager;
    void Start()
    {
        Debug.Log("Starting from test screen");
        _networkLaunchManager = GetComponent<NetworkLaunchManager>();
        
        PhotonNetwork.NickName = "TestUser";
        _networkLaunchManager.ConnectToPhotonServer();
    }

    public void OnJoinRoomStart()
    {
        _networkLaunchManager.StartGame();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventsConstants.NOTIFY_IMPOSTOR)
        {
            _gameSceneManager.SpawnMe();
            

            if (IsImpostor)
            {
                SceneStateManager.Instance.SetImpostor(PhotonNetwork.LocalPlayer);
            }
            else
            {
                SceneStateManager.Instance.ResetPlayers();
                // SceneStateManager.Instance.AddPlayer(PhotonNetwork.LocalPlayer);
            }
        }
    }
}
