using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SabotagesManager : Singleton<SabotagesManager>
{
    public CooldownManager CooldownManager;
    public bool AreLightsOn;
    public bool IsOxygenOn;
    public bool IsReactorOn;
    
    private RaiseEventOptions _raiseEventOptions;
    private void Start()
    {
        _raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
    }

    public void SabotageLights()
    {
        if(CanSabotage()){
            PhotonNetwork.RaiseEvent(EventsConstants.SABOTAGE_LIGHT, null, _raiseEventOptions, SendOptions.SendReliable);
        }
    }
    
    public void FixLights()
    {
        PhotonNetwork.RaiseEvent(EventsConstants.FIX_LIGHT, null, _raiseEventOptions, SendOptions.SendReliable);
    }

    public void SabotageReactor()
    {
        
    }

    public void SabotageOxygen()
    {
        
    }
    
    public void FixReactor()
    {
        
    }

    public void FixOxygen()
    {
        
    }

    public bool CanSabotage()
    {
        return !IsSabotaged() && CooldownManager.GetTimer("SabotageCooldown") <= 0;
    }

    public void TurnOffLights()
    {
        AreLightsOn = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.GetComponent<MomongoController>().TurnOffLights();
        }
    }

    public void TurnOnLights()
    {
        AreLightsOn = true;
        CooldownManager.AddTimer("SabotageCooldown", (int) PhotonNetwork.CurrentRoom.CustomProperties["SabotageLightsCooldown"]);
    }

    public bool IsSabotaged()
    {
        return !AreLightsOn || !IsOxygenOn || !IsReactorOn ;
    }
}
