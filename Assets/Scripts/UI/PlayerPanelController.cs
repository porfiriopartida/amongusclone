using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerPanelController : MonoBehaviour
    {
        public GameObject VotePanel;
        public GameObject PlayerVoted;
        public Image PlayerIcon;
        public Text PlayerName;

        private PlayerWrapper _player;

        public Sprite DeadSprite;
        public Sprite AliveSprite;

        public Button ShowVotingButtonsButton; 

        public void SetPlayer(PlayerWrapper player, bool isAlive)
        {
            this._player = player;
            PlayerIcon.color = player.Color;
            
            PlayerName.text = player.Player.NickName;
            if (SceneStateManager.Instance.IsImpostor() && player.IsImpostor)
            {
                PlayerName.color = Color.red;
                PlayerName.fontStyle = FontStyle.Bold;
            }
            else
            {
                PlayerName.color = Color.black;
                PlayerName.fontStyle = FontStyle.Normal;
            }

            // Evaluate(isAlive);
        }

        // Evaluates the state of the voting player panel, if sets the user vote to false
        // If the player is a ghost they cannot be voted
        // If the local Player is dead then they cannot cast votes
        public void InitializeUI(bool isLocalAlive)
        {
            PlayerIcon.sprite = _player.IsAlive ? AliveSprite : DeadSprite;
            
            if (isLocalAlive && _player.IsAlive)
            {
                EnableButton();
            } else {
                DisableButton();
            }
            this.PlayerVoted.SetActive(false);
        }

        public void DisableButton()
        {
            HideVoteActions();
            ShowVotingButtonsButton.enabled = false;
        }
        public void EnableButton()
        {
            HideVoteActions();
            ShowVotingButtonsButton.enabled = true;
        }

        public void ShowVoteActions()
        {
            if (SceneStateManager.Instance.IsAlive())
            {
                VotePanel.SetActive(true);
            }
            else
            {
                VotePanel.SetActive(false);
            }
        }

        public void HideVoteActions()
        {
            VotePanel.SetActive(false);
            this.VotePanel.SetActive(false);
        }

        public void VoteThis()
        {
            if (this._player.IsAlive)
            {
                VotingManager.Instance.CastVote(this._player.Player.UserId);
            }
        }

        public void SetVoted(string uuid)
        {
            if (String.IsNullOrEmpty(uuid))
            {
                return;
            }

            if (this._player.Player.UserId.Equals(uuid))
            {
                this.PlayerVoted.SetActive(true);
            }
        }
    }
}
