using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EmergencyButtonInteractable : UseInteractable
{
    public CooldownManager CooldownManager;
    private bool CanUse()
    {
        if (CooldownManager.GetTimer("EmergencyCooldown") > 0)
        {
            return false;
        }
        return 
            SceneStateManager.Instance.EmergencyCount > 0
            && SceneStateManager.Instance.IsAlive() 
            && !SabotagesManager.Instance.IsSabotaged();
    }

    public override void Interact()
    {
        if (CanUse())
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.EmergencyButtonPressed, PhotonNetwork.LocalPlayer.UserId, raiseEventOptions, SendOptions.SendReliable);
            
            SceneStateManager.Instance.EmergencyCount--;
        }
        else
        {
            Debug.Log("You are dead, stop pressing the button!");
        }
    }
}
