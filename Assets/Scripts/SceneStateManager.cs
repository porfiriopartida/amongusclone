using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using PUN;
using UI;
using UnityEngine;

public class SceneStateManager : Singleton<SceneStateManager>
{
    public CooldownManager CooldownManager;
    public GameEvent HardEvent;
    public SceneState SceneState;
    public GameConfiguration gameConfiguration;
    public CharacterColors CharacterColors;
    public InputController InputController;

    public GameObject Spawn;
    
    public static string TAG_PLAYER = "Player";

    public GameEvent NotReportable;
    private MomongoController _momongoController;

    private GameObject[] players;
    public MomongoController MomongoController
    {
        get => _momongoController;
        set
        {
            _momongoController = value;
            InputController = _momongoController.GetComponent<InputController>();
        }
    }

    public bool IsGameRunning { get; set; }
    public int EmergencyCount = 0;

    public void EnteringMiniGame()
    {
        MomongoController.Stop();
        DisableRegularInput();
    }

    public void DisableRegularInput()
    {
        InputController.enabled = false;
    }

    public void EnableRegularInput()
    {
        InputController.enabled = true;
    }

    public void LeavingMiniGame()
    {
        InputController.enabled = true;
    }

    // public void SetPlayers(Player[] players)
    // {
    //     SceneState.SetPlayers(players);
    // }
    public void SetImpostor(Player player)
    {
        SceneState.SetImpostor(player);
    }
    public void ResetPlayers()
    {
        SceneState.ResetPlayers();
    }


    public bool IsImpostor(Player localPlayer)
    {
        return SceneState.IsImpostor(localPlayer);
    }
    public bool IsImpostor()
    {
        //TODO: Cache LocalPlayer
        return SceneState.IsImpostor(PhotonNetwork.LocalPlayer);
    }

    public int GetImpostorsCount()
    {
        return SceneState.GetImpostorsCount();
    }
    public int GetCrewmateCount()
    {
        return SceneState.GetCrewmateCount();
    }
    public int GetTotalTasksCount()
    {
        return SceneState.GetTotalTasksCount();
    }
    public int GetCompletedTasksCount()
    {
        return SceneState.GetCompletedTasksCount();
    }

    public Hashtable ParseProperties()
    {
        return gameConfiguration.toHashtable();
    }

    public Color GetColor(Player player)
    {
        return SceneState.GetColor(player);
    }

    public void SetColor(Player player, int idx)
    {
        Debug.Log("Assigning color to " + player.NickName + ":" + idx);
        SceneState.SetColor(player, idx);
    }

    public void SetColor(Player player)
    {
        var playerIndex = SceneState.GetPlayerIndex(player);
        SetColor(player, playerIndex);
    }

    public void SyncGhosts()
    {
        bool isLocalPlayerAlive = SceneState.IsAlive(PhotonNetwork.LocalPlayer);
        
        GameObject[] playersGameObjects = GameObject.FindGameObjectsWithTag(TAG_PLAYER);
        foreach (var player in playersGameObjects)
        {
            MomongoController momongoController = player.GetComponent<MomongoController>();
            Player owner = momongoController.photonView.Owner;
            bool isRemotePlayerAlive = SceneState.IsAlive(owner);
            bool isEnabled = (owner.IsLocal || !isLocalPlayerAlive || isRemotePlayerAlive);
           
            momongoController.GetComponent<SpriteRenderer>().enabled = isEnabled;
            momongoController.AdminSprite.GetComponent<SpriteRenderer>().enabled = isEnabled;
            momongoController.GetComponent<PlayerSetup>().PlayerNamePanel.SetActive(isEnabled);
        }
    }

    public void SetIsAlive(Player player, bool b)
    {
        SceneState.SetIsAlive(player, b);
    }

    public bool IsAlive()
    {
        return SceneState.IsAlive(PhotonNetwork.LocalPlayer);
    }
    public bool IsAlive(Player player)
    {
        return SceneState.IsAlive(player);
    }

    private void HideAllPlayers()
    {
        HardEvent.Raise();
        players = GameObject.FindGameObjectsWithTag(TAG_PLAYER);
        foreach (var _gameObject in players)
        {
            _gameObject.SetActive(false);
        }
    }

    private void ShowAllPlayers()
    {
        foreach (var _gameObject in players)
        {
            _gameObject.SetActive(true);
        }
    }

    public void EnteringVoting()
    {
        DisableRegularInput();
        HideAllPlayers();
    }
    public void LeavingVoting()
    {
        ShowAllPlayers();
        
        TeleportPlayers(Vector3.zero);
        
        EnableRegularInput();
        
        VotingManager.Instance.StopVoting();
        
        //Resets the reportable to not reportable (no dead bodies are found at this stage)
        NotReportable.Raise();
    }

    private void TeleportPlayers(Vector3 position)
    {
        // GameObject[] players = GameObject.FindGameObjectsWithTag(TAG_PLAYER);
        foreach (var playerGameObject in players)
        {
            playerGameObject.transform.position = position;
        }
    }

    public Player FindPlayer(string uuid)
    {
        return SceneState.FindPlayer(uuid);
    }

    public void CleanBodies()
    {
        GameObject[] deadBodies = GameObject.FindGameObjectsWithTag("DeadBody");
        foreach (var deadBody in deadBodies)
        {
            Destroy(deadBody);
        }
    }

    public void SkipVoted()
    {
        GameObject[] deadPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var deadPlayer in deadPlayers)
        {
            MomongoController momongoController = deadPlayer.GetComponent<MomongoController>();
            Player player = momongoController.photonView.Owner;
            if (!IsAlive(player))
            {
                momongoController.GhostMe();
            }
            else
            {
                momongoController.ResetCooldowns();
            }
        }
    }
    public void KillVoted(string uuid)
    {
        GameObject[] deadPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (var deadPlayer in deadPlayers)
        {
            MomongoController momongoController = deadPlayer.GetComponent<MomongoController>();
            Player player = momongoController.photonView.Owner;
            string userId = player.UserId;
            if ( userId.Equals(uuid))
            {
                momongoController.Die();
            } else if (!IsAlive(player))
            {
                momongoController.GhostMe();
            }
            else
            {
                momongoController.ResetCooldowns();
            }
        }
    }

    public void AwardProgress()
    {
        if (CooldownManager.GetTimer("AwardCooldown") <= 0)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.TASK_COMPLETE, PhotonNetwork.LocalPlayer.UserId, raiseEventOptions, SendOptions.SendReliable);
            CooldownManager.AddTimer("AwardCooldown", 2);
        }
    }

    public void RemoveAllUsers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
    }
}
