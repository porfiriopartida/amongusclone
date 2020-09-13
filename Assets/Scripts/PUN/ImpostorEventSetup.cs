using System.Collections;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ImpostorEventSetup : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameEvent NotifyImpostor;
    public GameEvent NotifyCrewmate;
    private void EventSetupImpostor(string[] userIdList)
    {
        SceneStateManager.Instance.ResetPlayers();
        foreach (var userId in userIdList)
        {
            PlayerWrapper playerWrapper = SceneStateManager.Instance.FindPlayer(userId);
            SceneStateManager.Instance.SetImpostor(playerWrapper.Player);
            Debug.Log("Impostor is: " + playerWrapper.Player.NickName + ":" + userId);
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

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventsConstants.NotifyImpostor)
        {
            EventSetupImpostor((string[])photonEvent.CustomData);
        }
    }
}