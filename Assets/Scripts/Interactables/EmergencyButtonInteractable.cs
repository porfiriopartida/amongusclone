using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EmergencyButtonInteractable : UseInteractable
{
    public override void Interact()
    {
        if (SceneStateManager.Instance.IsAlive())
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.EmergencyButtonPressed, PhotonNetwork.LocalPlayer.UserId, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Debug.Log("You are dead, stop pressing the button!");
        }
    }
}
