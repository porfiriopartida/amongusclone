using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DeadBodyInteractable : ReportInteractable
{
    private PlayerWrapper _player;
    
    public override void Interact()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " found a dead body.");
    }

    public void SetPlayer(Player deadPlayer)
    {
        this._player = SceneStateManager.Instance.GetPlayerWrapper(deadPlayer);
    }

    public override void Interact(Player player)
    {
        string bodyNickname = "<PlayerName>";
        if (_player != null)
        {
            bodyNickname = _player.Player.NickName;
        }
        #if UNITY_EDITOR
        else
        {
            _player = new PlayerWrapper();
            _player.Color = Color.green;
            _player.Player = PhotonNetwork.LocalPlayer;
        }
        #endif

        Debug.Log(player.NickName + " found the dead body of " + bodyNickname);
        
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
         
        PhotonNetwork.RaiseEvent(EventsConstants.ReportDead, new string[]{PhotonNetwork.LocalPlayer.UserId, _player.Player.UserId}, raiseEventOptions, SendOptions.SendReliable);
    }
}