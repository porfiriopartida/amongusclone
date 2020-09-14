using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using UI;
using UnityEngine;

public class VotingEventListener : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject BodyFoundPanel;
    public GameObject EmergencyButtonPressedAlert;
    private bool _processing = false;
    public void OnEvent(EventData photonEvent)
    {
        byte code = photonEvent.Code;
        if (code == EventsConstants.REPORT_DEAD)
        {
            //Show Body Found Screen:
            SceneStateManager.Instance.EnteringVoting();
            
            DeadBodyReport deadBodyReport = new DeadBodyReport((string[]) photonEvent.CustomData);
            if (deadBodyReport.Reporter != null)
            {
                BodyFoundPanel.GetComponent<BodyFoundPanelController>().SetFounder(deadBodyReport.Reporter);
            }
            if (deadBodyReport.Body != null)
            {
                BodyFoundPanel.GetComponent<BodyFoundPanelController>().SetBody(deadBodyReport.Body);
            }
            
            BodyFoundPanel.SetActive(true);
            
            //TODO: Body Found SFX - dead found
            
            StartCoroutine(ShowVotingScreen());
        }
        else if (code == EventsConstants.EMERGENCY_BUTTON_PRESSED)
        {
            //Show Body Found Screen:
            SceneStateManager.Instance.EnteringVoting();
            string reporterdUuid = (string) photonEvent.CustomData;
            Player Reporter = SceneStateManager.Instance.FindPlayer(reporterdUuid);
            Debug.Log(Reporter.NickName + " called for an emergency meeting");
            Debug.Log(reporterdUuid);
            EmergencyButtonPressedAlert.GetComponent<EmergencyController>().SetFounder(Reporter);
            EmergencyButtonPressedAlert.SetActive(true);
            
            //TODO: Body Found SFX - Emergency called
            
            StartCoroutine(ShowVotingScreen());
        }
        else if (code == EventsConstants.VOTED_SKIP)
        {
            // Debug.Log("Player just voted: " + photonEvent.CustomData);
            // Debug.Log("- - Skipped");
            string voterUuid = (string) photonEvent.CustomData;
            VotingManager.Instance.RegisterVote("SKIP", voterUuid);
            ShowVotedIcon(voterUuid);
            ValidateVotes();
        }
        else if (code == EventsConstants.VOTED_PLAYER)
        {
            string[] voterData = (string[]) photonEvent.CustomData;
            string userCastingVote = voterData[0];
            string userGettingVoted = voterData[1];
            // Debug.Log("Player just voted: " + voterData[0] );
            // Debug.Log("- - voted for: " + voterData[1] );
            VotingManager.Instance.RegisterVote(userGettingVoted, userCastingVote);
            ShowVotedIcon(voterData[0]);
            ValidateVotes();
        }
        else if (code == EventsConstants.VOTE_PROCESS)
        {
            if (_processing)
            {
                //Already processing by a different event (time/allvoted)
                return;
            }

            _processing = true;
            
            VotingManager.Instance.ShowVoters();

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(KickMostVoted());
            }
            
            _processing = false;
        }
        else if (code == EventsConstants.PROCESS_SKIP)
        {
            SceneStateManager.Instance.LeavingVoting();
            SceneStateManager.Instance.SkipVoted();
        }
        else if (code == EventsConstants.PROCESS_KICK)
        {
            string uuid = (string) photonEvent.CustomData;
            SceneStateManager.Instance.LeavingVoting();
            SceneStateManager.Instance.KillVoted(uuid);
        }
    }

    private IEnumerator KickMostVoted()
    {
        yield return new WaitForSeconds(2);
        
        Player voted = VotingManager.Instance.GetMostVoted();
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        
        if (voted == null)
        {
            PhotonNetwork.RaiseEvent(EventsConstants.PROCESS_SKIP, null, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            PhotonNetwork.RaiseEvent(EventsConstants.PROCESS_KICK, voted.UserId, raiseEventOptions, SendOptions.SendReliable);
        }
    }
    private void ValidateVotes()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Player[] players = PhotonNetwork.PlayerList;
            bool allAliveVoted = true;
            foreach (var player in players)
            {
                if (SceneStateManager.Instance.IsAlive(player))
                {
                    if (!VotingManager.Instance.HasAlreadyVoted(player.UserId))
                    {
                        allAliveVoted = false;
                    }
                }
            }

            if (allAliveVoted)
            {
                VotingManager.Instance.ProcessVoting();
            }
        }
    }
    private IEnumerator ShowVotingScreen()
    {
        yield return new WaitForSeconds(2);
        BodyFoundPanel.SetActive(false);
        EmergencyButtonPressedAlert.SetActive(false);
        VotingManager.Instance.StartDiscussionPanel();
    }
    private void ShowVotedIcon(string voterUuid)
    {          
        GameObject[] playersPanels = GameObject.FindGameObjectsWithTag("PlayerVotingPanel");
        foreach (var playersPanel in playersPanels)
        {
            PlayerPanelController playerPanelController = playersPanel.GetComponent<PlayerPanelController>();
            playerPanelController.SetVoted(voterUuid);
        }
    }
}
