using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VotingTimer : MonoBehaviour
    {
        private Text timerText;
        private CooldownManager _cooldownManager;
        private string _votingCooldown = "votingTimer";
        private bool IsRunning = true;

        public void ResetTimer(float timer)
        {
            this.timerText = GetComponent<Text>();
            this._cooldownManager = GetComponent<CooldownManager>();
            this._cooldownManager.Initialize();
            _cooldownManager.AddTimer(_votingCooldown, timer);
            IsRunning = true;
        }

        void Update()
        {
            if (!IsRunning)
            {
                return;
            }

            if(_cooldownManager.GetTimer(_votingCooldown)>=0){
                timerText.text = Mathf.Floor(_cooldownManager.GetTimer(_votingCooldown)).ToString();
            }
            else
            {
                timerText.text = "0";
                IsRunning = false;


                VotingManager.Instance.ProcessVoting();
                this.enabled = false;
            }
        }
    }
}
