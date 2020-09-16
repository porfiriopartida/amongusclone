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
        if (player == null)
        {
            return;
        }

        player.CustomProperties[CustomProperties.PLAYER_COLOR] = idx;
        player.SetCustomProperties(player.CustomProperties);
    }
    
    public Color GetColor(Player player)
    {
        int idx = (int)player.CustomProperties[CustomProperties.PLAYER_COLOR];
        Color c = CharacterColors.colors[idx];
        return c;
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
