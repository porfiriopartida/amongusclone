using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DeadBodyInteractable : ReportInteractable
{
    private Player _player;
    
    public override void Interact()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " found a dead body.");
    }

    public void SetPlayer(Player deadPlayer)
    {
        this._player = deadPlayer;
    }

    public override void Interact(Player player)
    {
        string bodyNickname = "<PlayerName>";
        if (_player != null)
        {
            bodyNickname = _player.NickName;
        }
        #if UNITY_EDITOR
        else
        {
            _player = PhotonNetwork.LocalPlayer;
            // SceneStateManager.Instance.SetColor(player, Color.green);
        }
        #endif

        Debug.Log(player.NickName + " found the dead body of " + bodyNickname);
        
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
         
        PhotonNetwork.RaiseEvent(EventsConstants.ReportDead, new string[]{PhotonNetwork.LocalPlayer.UserId, _player.UserId}, raiseEventOptions, SendOptions.SendReliable);
    }
}