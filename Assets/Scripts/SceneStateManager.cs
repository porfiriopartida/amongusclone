using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using PUN;
using UI;
using UnityEngine;

public class SceneStateManager : Singleton<SceneStateManager>
{
    public SceneState SceneState;
    public GameConfiguration gameConfiguration;
    public CharacterColors CharacterColors;
    public InputController InputController;

    public GameObject Spawn;
    
    public static string TAG_PLAYER = "Player";

    public GameEvent NotReportable;
    public MomongoController MomongoController
    {
        set
        {
            this.InputController = value.GetComponent<InputController>();
        }
    }

    public void EnteringMiniGame()
    {
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

    public void SetPlayers(Player[] players)
    {
        SceneState.SetPlayers(players);
    }
    public void SetImpostor(Player player)
    {
        SceneState.SetImpostor(player);
    }
    public void ResetPlayers()
    {
        SceneState.ResetPlayers();
    }

    public void AddPlayer(Player player)
    {
        SceneState.AddPlayer(player);
    }

    public void RemovePlayer(Player player)
    {
        SceneState.RemovePlayer(player);
    }

    public void Clear()
    {
        SceneState.Clear();
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
        var playerIndex = SceneState.GetPlayerIndex(player);
        SceneState.GetPlayer(player).Color = CharacterColors.colors[playerIndex];
        Debug.Log(SceneState.GetPlayer(player).Color);
        return SceneState.GetPlayer(player).Color;
    }
    public PlayerWrapper GetPlayerWrapper(Player player)
    {
        return SceneState.GetPlayer(player);
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

    private GameObject[] players;
    private void HideAllPlayers()
    {
        players = GameObject.FindGameObjectsWithTag(TAG_PLAYER);
        foreach (var gameObject in players)
        {
            gameObject.SetActive(false);
        }
    }

    private void ShowAllPlayers()
    {
        foreach (var gameObject in players)
        {
            gameObject.SetActive(true);
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

    public List<PlayerWrapper> GetPlayers()
    {
        return SceneState.GetPlayers();
    }

    public PlayerWrapper FindPlayer(string uuid)
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
            if (!GetPlayerWrapper(player).IsAlive)
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
            } else if (!GetPlayerWrapper(player).IsAlive)
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
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventsConstants.TASK_COMPLETE, PhotonNetwork.LocalPlayer.UserId, raiseEventOptions, SendOptions.SendReliable);
    }

    public void RemoveAllUsers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }
    }
}
