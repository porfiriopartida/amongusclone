using System;
using System.Collections.Generic;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI
{
    public class VotingManager : Singleton<VotingManager>
    {
        public GameObject DiscussionPanel;
        private DiscussionPanelManager DiscussionPanelManager;
        private bool _started = false;
        public VotingTimer VotingTimer;
        public GameObject SkipButton;
        public Hashtable Votes;
        private List<string> AlreadyVoted;

        // private Dictionary<string, int>
        private bool _isAlive;
        private void Start()
        {
            DiscussionPanelManager = DiscussionPanel.GetComponent<DiscussionPanelManager>();
            DiscussionPanel.SetActive(false);
        }

        private List<GameObject> playersPanels;

        public void StartDiscussionPanel()
        {
            _isAlive = SceneStateManager.Instance.IsAlive();
            if (!_started)
            {
                playersPanels = new List<GameObject>();
                _started = true;
                List<PlayerWrapper> players = SceneStateManager.Instance.GetPlayers();
                foreach (var player in players)
                {
                    playersPanels.Add(DiscussionPanelManager.AddPlayer(player, _isAlive));
                }
                //Cannot use with tag if they are disabled.
                // playersPanels = GameObject.FindGameObjectsWithTag("PlayerVotingPanel");
            }
            EnableVotes();

            StartVoting();
        }


        public void RegisterVote(string myVote, string playerVoting)
        {
            Debug.Log(myVote + " got voted by " + playerVoting);
            if (HasAlreadyVoted(playerVoting))
            {
                Debug.Log(playerVoting + " HasAlreadyVoted ");
                return;
            }

            AlreadyVoted.Add(playerVoting);
            object voters;
            if (!Votes.TryGetValue(myVote, out voters))
            {
                voters = new List<string>();
                Votes.Add(myVote, voters);
            }
            ((List<string>)Votes[myVote]).Add(playerVoting);
        }

        public bool HasAlreadyVoted(string uuid)
        {
            return AlreadyVoted.Contains(uuid);
        }

        public void StartVoting()
        {
            SceneStateManager.Instance.CleanBodies();
            
            Votes = new Hashtable();
            AlreadyVoted = new List<string>();
            
            VotingTimer.gameObject.SetActive(true);
            VotingTimer.enabled = true;
            VotingTimer.ResetTimer((float) PhotonNetwork.CurrentRoom.CustomProperties["VoteTime"]);
            DiscussionPanel.SetActive(true);
        }
        
        private void EnableVotes()
        {
            SkipButton.SetActive(_isAlive);
            // GameObject[] playersPanels = GameObject.FindGameObjectsWithTag("PlayerVotingPanel");
            foreach (var playersPanel in playersPanels)
            {
                PlayerPanelController playerPanelController = playersPanel.GetComponent<PlayerPanelController>();
                // playerPanelController.EnableButton();
                playerPanelController.InitializeUI(_isAlive);
            }
        }
        
        private void DisableVotes()
        {          
            SkipButton.SetActive(false);
            // GameObject[] playersPanels = GameObject.FindGameObjectsWithTag("PlayerVotingPanel");
            foreach (var playersPanel in playersPanels)
            {
                PlayerPanelController playerPanelController = playersPanel.GetComponent<PlayerPanelController>();
                playerPanelController.DisableButton();
            }
        }
        public void StopVoting()
        {
            DiscussionPanel.SetActive(false);
        }
        public void SkipVote()
        {
            if (!SceneStateManager.Instance.IsAlive())
            {
                return;
            }
            Debug.Log("Skip Voted");
            DisableVotes();
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.VOTED_SKIP, PhotonNetwork.LocalPlayer.UserId, raiseEventOptions, SendOptions.SendReliable);
        }
        public void CastVote(String player)
        {
            if (!SceneStateManager.Instance.IsAlive())
            {
                return;
            }
            Debug.Log("Voted for...");
            DisableVotes();
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(EventsConstants.VOTED_PLAYER, new string[]{ PhotonNetwork.LocalPlayer.UserId,  player }, raiseEventOptions, SendOptions.SendReliable);
        }
        public void ProcessVoting()
        {
            Debug.Log("Processing votes...");
            VotingTimer.gameObject.SetActive(false);
            if (PhotonNetwork.IsMasterClient)
            {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
                PhotonNetwork.RaiseEvent(EventsConstants.VOTE_PROCESS, null, raiseEventOptions,
                    SendOptions.SendReliable);
            }
        }
        public void ShowVoters()
        {
            DisableVotes();
            List<PlayerWrapper> playerWrappers = SceneStateManager.Instance.GetPlayers();
            foreach (var playerWrapper in playerWrappers)
            {
                string currentNickname = playerWrapper.Player.NickName;
                object voters;
                if (Votes.TryGetValue(playerWrapper.Player.UserId, out voters))
                {
                    List<string> voterList = (List<string>) voters;
                    Debug.Log("Users that voted for " + currentNickname + ": ");
                    foreach (string voter in voterList)
                    {
                        Debug.Log("--" + voter);
                        //TODO: Show voters icons/delayed?
                    }
                }
            }
        }
        public PlayerWrapper GetMostVoted()
        {
            List<PlayerWrapper> playerWrappers = SceneStateManager.Instance.GetPlayers();

            object voters;
            int currentMax = -1;
            bool IsTie = false;
            string currentNickname = "SKIP";
            PlayerWrapper currentPlayer = null;
            if (Votes.TryGetValue("SKIP", out voters))
            {
                List<string> voterList = (List<string>) voters;
                currentMax = voterList.Count;
            }
            else
            {
                currentMax = 0;
            }

            foreach (var playerWrapper in playerWrappers)
            {
                if (Votes.TryGetValue(playerWrapper.Player.UserId, out voters))
                {
                    List<string> voterList = (List<string>) voters;
                    if (currentMax == voterList.Count )
                    {
                        IsTie = true;
                    }
                    else if(voterList.Count > currentMax)
                    {
                        currentNickname = playerWrapper.Player.NickName;
                        currentMax = voterList.Count;
                        currentPlayer = playerWrapper;
                        IsTie = false;
                    }
                }
            }


            if (IsTie || "SKIP".Equals(currentNickname))
            {
                Debug.Log("NO ONE WAS KICKED");
                return null;
            }

            Debug.Log(currentPlayer.Player.NickName + " is getting kicked");
            return currentPlayer;
        }
    }
}
