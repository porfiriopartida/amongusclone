using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

[CreateAssetMenu(menuName = "Momongos/SceneState")]
public class SceneState : ScriptableObject
{
    private List<PlayerWrapper> playerWrapper;

    public List<PlayerWrapper> GetPlayers()
    {
        return playerWrapper;
    }
    //public PlayerConfiguration playerConfiguration;
    //public ImpostorConfiguration impostorConfiguration;
    //public MapConfiguration mapConfiguration;

    public void SetPlayers(Player[] players)
    {
        playerWrapper = new List<PlayerWrapper>();
        for (var index = 0; index < players.Length; index++)
        {
            var player = players[index];
           // bool isImpostor = impostors.Contains(index);
            playerWrapper.Add(PlayerWrapper.Build(player));
        }
    }

    public void ResetPlayers()
    {
        foreach (var _playerWrapper in playerWrapper)
        {
            _playerWrapper.IsImpostor = false;
            _playerWrapper.IsAlive = true;
        }
    }
    public bool IsImpostor(Player player)
    {
        return GetPlayer(player).IsImpostor;
    }

    public void SetImpostor(Player player)
    {
        if (player == null)
        {
            return;
        }
        
        PlayerWrapper playerWrapper = GetPlayer(player);
        if (playerWrapper != null)
        {
            playerWrapper.IsImpostor = true;
        }
    }
    
    
    public void SetColor(Player player, Color color)
    {
        if (player == null)
        {
            return;
        }
        
        PlayerWrapper playerWrapper = GetPlayer(player);
        if (playerWrapper != null)
        {
            playerWrapper.Color = color;
        }
    }

    public PlayerWrapper GetPlayer(Player player)
    {
        foreach (var _playerWrapper in playerWrapper)
        {
            if (_playerWrapper.Player.UserId.Equals(player.UserId))
            {
                return _playerWrapper;
            }
        }

        return null;
    }

    public void Clear()
    {
        playerWrapper = new List<PlayerWrapper>();
    }

    public void AddPlayer(Player player)
    {
        if (GetPlayer(player) != null)
        {
            return;
        }

        playerWrapper.Add(PlayerWrapper.Build(player));
    }

    public void RemovePlayer(Player player)
    {
        playerWrapper.Remove(GetPlayer(player));
    }

    public int GetImpostorsCount()
    {
        int c = 0;
        foreach (var wrapper in playerWrapper)
        {
            if (wrapper.IsImpostor && wrapper.IsAlive)
            {
                c++;
            }
        }

        return c;
    }
    public int GetCrewmateCount()
    {
        int c = 0;
        foreach (var wrapper in playerWrapper)
        {
            if (!wrapper.IsImpostor && wrapper.IsAlive)
            {
                c++;
            }
        }

        return c;
    }

    public int GetPlayerIndex(Player player)
    {
        for (var index = 0; index < playerWrapper.Count; index++)
        {
            var wrapper = playerWrapper[index];
            if (player.UserId.Equals(wrapper.Player.UserId))
            {
                return index;
            }
        }
        return -1;
    }

    public void SetIsAlive(Player player, bool isAlive)
    {
        GetPlayer(player).IsAlive = isAlive;
    }

    public bool IsAlive(Player player)
    {
        return GetPlayer(player).IsAlive && !player.IsInactive;
    }

    public PlayerWrapper FindPlayer(string uuid)
    {
        for (var index = 0; index < playerWrapper.Count; index++)
        {
            var wrapper = playerWrapper[index];
            if (uuid.Equals(wrapper.Player.UserId))
            {
                return wrapper;
            }
        }
        return null;
    }

    public int GetTotalTasksCount()
    {
        return 0;
    }

    public int GetCompletedTasksCount()
    {
        return 0;
    }
}

[Serializable]
public class PlayerWrapper
{
    public Player Player;
    public bool IsImpostor;
    public bool IsAlive;
    public Color Color { get; set; }

    public static PlayerWrapper Build(Player player)
    {
        PlayerWrapper playerWrapper = new PlayerWrapper();
        playerWrapper.Player = player;
        playerWrapper.IsAlive = true;
        return playerWrapper;
    }
}
