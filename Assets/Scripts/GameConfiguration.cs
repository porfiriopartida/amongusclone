using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Momongos/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{  
    [NonSerialized]
    public float ChangeSceneWaiting;
    [NonSerialized]
    public float ChangeSceneWaitingDebug;
    [NonSerialized]
    public float MinCrewmate;

    public bool IsDebug;
    public int[] DebugImpostors;
    
    [Header("UI Configurables")]
    [Space(10, order = -1)]
    [Header("- Main Configurations")]
    [Range(1, 5)]
    public int TotalImpostors = 1;
    // public bool ConfirmEjects = true;
    // public bool ConfirmAnimations = true;
    public int EmergencyCount = 1;
    public int EmergencyCooldown = 15;
    
    [Space(5, order = -1)]
    [Header("Players Speed")]
    [Range(0.25f, 5f)]
    public float MovSpeed = 1;
    
    [Space(5, order = -1)]
    [Header("- Discussion:")]
    // [SerializeField]
    // public float EmergencyMeetingCooldown = 15;
    // public float DiscussTime = 30;
    public float VoteTime = 90;
    
    
    [Space(5, order = -1)]
    [Header("- Crewmates:")]
    public float VisionRange = 1;
    
    [Space(5, order = -1)]
    [Header("- Impostors:")]
    public float ImpostorVisionRange = 1;
    [Min(10)]
    public float KillCooldown = 15;
    // public float SabotageHardCooldown = 15;
    public int SabotageLightsCooldown = 15;
    // public float SabotageDoorsCooldown = 10;
    
    [Space(5, order = -1)]
    [Header("- Tasks:")]
    [Min(0)]
    public int LongTask = 1;
    public int MidTask = 2;
    public int ShortTask = 4;
        
    [Space(5, order = -1)]
    [Header("- Sabotage Effects")] 
    // [Min(10)]
    // public float SabotageTimeToDie = 30;
    // [Min(1)]
    // public float SabotageDoorsClosedTime = 5;
    [Min(0)]
    public float SabotageLightsOffDelay = 5;
    public  bool SabotageLightFadeEffect = true;
    public Hashtable toHashtable()
    {
        Hashtable hashtable = new Hashtable();
        
        hashtable.Add("TotalImpostors", TotalImpostors);
        
        // hashtable.Add("ConfirmEjects", ConfirmEjects);
        // hashtable.Add("ConfirmAnimations", ConfirmAnimations);
        //
        // hashtable.Add("EmergencyMeetingsCount", EmergencyMeetingsCount);
        // hashtable.Add("EmergencyMeetingCooldown", EmergencyMeetingCooldown);
        //
        // hashtable.Add("DiscussTime", DiscussTime);
        hashtable.Add("VoteTime", VoteTime);

        // float movSpeed = (float) PhotonNetwork.CurrentRoom.CustomProperties["MovSpeed"];
        hashtable.Add("MovSpeed", MovSpeed);
        
        hashtable.Add("VisionRange", VisionRange);
        hashtable.Add("ImpostorVisionRange", ImpostorVisionRange);
        hashtable.Add("KillCooldown", KillCooldown);
        
        hashtable.Add("LongTask", LongTask);
        hashtable.Add("MidTask", MidTask);
        hashtable.Add("ShortTask", ShortTask);

        int TotalPlayers = PhotonNetwork.PlayerList.Length;
        hashtable.Add("TotalTasks", (LongTask +  MidTask + ShortTask) * (TotalPlayers - TotalImpostors));
        
        hashtable.Add("SabotageLightsCooldown", SabotageLightsCooldown);
        
        hashtable.Add("EmergencyCount", EmergencyCount);
        hashtable.Add("EmergencyCooldown", EmergencyCooldown);
        return hashtable;
    }
}