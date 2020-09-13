using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Utils
{
    public class AmongosRandomGenerator
    {
        //TODO: Use UUID instead of the index position.
        public static string[] GenerateImpostors(Player[] playerList, int total)
        {
//            Assert.IsTrue(total < playerList.Length);

            List<string> selectedList = new List<string>();

            while (selectedList.Count < total)
            {
                int rnd = Random.Range(0, PhotonNetwork.PlayerList.Length);
                string playerId = playerList[rnd].UserId;
                if (!selectedList.Contains(playerId))
                {
                    selectedList.Add(playerId);
                }
            }

            return selectedList.ToArray();
        }
    }
}