using System;
using Photon.Realtime;
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

        private Player _player;

        public Sprite DeadSprite;
        public Sprite AliveSprite;

        public Button ShowVotingButtonsButton; 

        public void SetPlayer(Player player, bool isAlive)
        {
            this._player = player;
            PlayerIcon.color = SceneStateManager.Instance.GetColor(player);
            
            PlayerName.text = player.NickName;
            if (SceneStateManager.Instance.IsImpostor() && SceneStateManager.Instance.IsImpostor(player))
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
            bool _playerIsAlive = SceneStateManager.Instance.IsAlive(_player);
            PlayerIcon.sprite = _playerIsAlive ? AliveSprite : DeadSprite;
            
            if (isLocalAlive && _playerIsAlive)
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
            if (SceneStateManager.Instance.IsAlive(_player))
            {
                VotingManager.Instance.CastVote(this._player.UserId);
            }
        }

        public void SetVoted(string uuid)
        {
            if (String.IsNullOrEmpty(uuid))
            {
                return;
            }

            if (this._player.UserId.Equals(uuid))
            {
                this.PlayerVoted.SetActive(true);
            }
        }
    }
}
