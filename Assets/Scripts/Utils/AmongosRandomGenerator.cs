using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Utils
{
    public class AmongosRandomGenerator
    {
        //TODO: Use UUID instead of the index position.
        public static string[] GenerateImpostors(Player[] playerList, int total)
        {
            List<string> selectedList = new List<string>();

            while (selectedList.Count < total)
            {
                int rnd = Random.Range(0, PhotonNetwork.PlayerList.Length);
                string playerId = playerList[rnd].UserId;
                if (!selectedList.Contains(playerId))
                {
                    selectedList.Add(playerId);
                }

                if (selectedList.Count == playerList.Length)
                {
                    break;
                }
            }
            return selectedList.ToArray();
        }

        public static GameObject[] RandomizeTasks(GameObject[] task, int total)
        {
            List<GameObject> selectedList = new List<GameObject>();

            while (selectedList.Count < total)
            {
                int rnd = Random.Range(0, task.Length);
                GameObject gameObject = task[rnd];
                if (!selectedList.Contains(gameObject))
                {
                    selectedList.Add(gameObject);
                }
            }
            return selectedList.ToArray();
        }
        
    }
}