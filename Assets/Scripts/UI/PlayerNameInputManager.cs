using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace PUN
{
    public class PlayerNameInputManager : MonoBehaviour
    {
        private InputField inputField;
        private void Start()
        {
            inputField = GetComponent<InputField>();
            inputField.text = PhotonNetwork.NickName;
        }

        public void SetPlayerName(string playerName) {
            if (string.IsNullOrEmpty(playerName)) {
                Debug.Log("Name cannot be empty");
                return;
            }
            PhotonNetwork.NickName = playerName.Trim();
        }
    }
}
