using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[CreateAssetMenu(menuName = "Momongos/SceneState")]
public class SceneState : ScriptableObject
{
    public CharacterColors CharacterColors;
    public void ResetPlayers()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (var index = 0; index < players.Length; index++)
        {
            var currentPlayer = players[index];
            SetCustomProperties(currentPlayer);
        }
    }

    private void SetCustomProperties(Player player)
    {
        Hashtable hashtable = new Hashtable();
        int idx = -1;
        if (player.CustomProperties.TryGetValue(CustomProperties.PLAYER_COLOR, out var val))
        {
            idx = (int) val;
        }
        else
        {
            idx = player.ActorNumber;
        }


        hashtable[CustomProperties.IS_IMPOSTOR] = false;
        hashtable[CustomProperties.IS_ALIVE] = true;
        hashtable[CustomProperties.PLAYER_COLOR] = idx;

        player.SetCustomProperties(hashtable);
    }
    
    public bool IsImpostor(Player player)
    {
        return player != null && !player.IsInactive && (bool)player.CustomProperties[CustomProperties.IS_IMPOSTOR];
    }

    public void SetImpostor(Player player)
    {
        if (player == null)
        {
            return;
        }
        
        player.CustomProperties[CustomProperties.IS_IMPOSTOR] = true;
        
        player.SetCustomProperties(player.CustomProperties);
    }


    public void SetColor(Player player, int idx)
    {
        RaiseEventOptions _raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(EventsConstants.SELECT_COLOR, new string[2]{ player.UserId, idx.ToString()}, _raiseEventOptions, SendOptions.SendReliable);
    }
    
    public Color GetColor(Player player)
    {
        int idx = GetColorIdx(player);
        if (idx == -1)
        {
            return Color.white;
        }
        Color c = CharacterColors.colors[idx];
        return c;
    }
    public int GetColorIdx(Player player)
    {
        var obj = player.CustomProperties[CustomProperties.PLAYER_COLOR];
        if (obj == null)
        {
            return -1;
        }
        return (int)obj;
    }

    public List<int> GetTakenColors()
    {
        Player[] players = PhotonNetwork.PlayerList;
        List<int> colors = new List<int>();
        foreach (var player in players)
        {
            colors.Add(GetColorIdx(player));
        }

        return colors;
    }

    public int GetImpostorsCount()
    {
        int c = 0;
        Player[] players = PhotonNetwork.PlayerList;
        for (var index = 0; index < players.Length; index++)
        {
            var currentPlayer = players[index];
            if ((bool)currentPlayer.CustomProperties[CustomProperties.IS_ALIVE] && (bool)currentPlayer.CustomProperties[CustomProperties.IS_IMPOSTOR])
            {
                c++;
            }
        }

        return c;
    }
    public int GetCrewmateCount()
    {
        int c = 0;
        Player[] players = PhotonNetwork.PlayerList;
        for (var index = 0; index < players.Length; index++)
        {
            var currentPlayer = players[index];
            if ((bool)currentPlayer.CustomProperties[CustomProperties.IS_ALIVE] && !((bool)currentPlayer.CustomProperties[CustomProperties.IS_IMPOSTOR]))
            {
                c++;
            }
        }

        return c;
    }

    public int GetPlayerIndex(Player player)
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (var index = 0; index < players.Length; index++)
        {
            var currentPlayer = players[index];
            if (currentPlayer.UserId.Equals(player.UserId))
            {
                return index;
            }
        }
        return -1;
    }

    public void SetIsAlive(Player player, bool isAlive)
    {
        if (player != null)
        {
            player.CustomProperties[CustomProperties.IS_ALIVE] = isAlive;
            player.SetCustomProperties(player.CustomProperties);
        }
    }

    public bool IsAlive(Player player)
    {
        return player != null && !player.IsInactive && (bool)player.CustomProperties[CustomProperties.IS_ALIVE];
    }


    public Player FindPlayer(string uuid)
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (var index = 0; index < players.Length; index++)
        {
            var currentPlayer = players[index];
            if (uuid.Equals(currentPlayer.UserId))
            {
                return currentPlayer;
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

public class CustomProperties
{
    public const string IS_ALIVE = "IsAlive";
    public const string IS_IMPOSTOR = "IsImpostor";
    public const string PLAYER_COLOR = "Color";
}
