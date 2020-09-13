using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        public Text RoomTitle;
        public Text PlayersList;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void SetRoomTitle(string str)
        {
            RoomTitle.text = str;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void RenderPlayers()
        {
            Player[] players = PhotonNetwork.PlayerList;

            string playersStr = "";
            foreach (var player in players)
            {
                playersStr += player.NickName + "\n";
            }

            PlayersList.text = playersStr;
        }
    }
}
