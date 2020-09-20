using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSetupEventListener : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameEvent NotifyImpostor;
    public GameEvent NotifyCrewmate;
    public GameEvent ColorUpdated;
    private void EventSetupImpostor(string[] userIdList)
    {
        SceneStateManager.Instance.ResetPlayers();
        foreach (var userId in userIdList)
        {
            Player player = SceneStateManager.Instance.FindPlayer(userId);
            SceneStateManager.Instance.SetImpostor(player);
            Debug.Log("Impostor is: " + player.NickName + ":" + userId);
        }

        if (SceneStateManager.Instance.IsImpostor(PhotonNetwork.LocalPlayer))
        {
            NotifyImpostor.Raise();
        }
        else
        {
            NotifyCrewmate.Raise();
        }
    }

    private void Start()
    {
        ColorUpdated.Raise();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventsConstants.NOTIFY_IMPOSTOR) {
            EventSetupImpostor((string[])photonEvent.CustomData);
        } else if (photonEvent.Code == EventsConstants.NOTIFY_COLOR) {
            ColorUpdated.Raise();
        } else if (photonEvent.Code == EventsConstants.SELECT_COLOR && PhotonNetwork.IsMasterClient) {
            string[] customData = (string[]) photonEvent.CustomData;
            string uuid = customData[0];
            string sIdx = customData[1];
            int idx = int.Parse(sIdx);
            Player player = SceneStateManager.Instance.FindPlayer(uuid);
                
            if (player == null)
            {
                return;
            }

            List<int> TakenColors = SceneStateManager.Instance.GetTakenColors();
            if (!TakenColors.Contains(idx))
            {
                player.CustomProperties[CustomProperties.PLAYER_COLOR] = idx;
                player.SetCustomProperties(player.CustomProperties);
                
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
                PhotonNetwork.RaiseEvent(EventsConstants.NOTIFY_COLOR, null, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }
}