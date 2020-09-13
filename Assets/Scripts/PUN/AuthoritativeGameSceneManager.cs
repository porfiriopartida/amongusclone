using System;
using System.Collections;
using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PUN
{
    public class AuthoritativeGameSceneManager : MonoBehaviourPunCallbacks
    {
        public GameConfiguration GameConfiguration;

        public Integer TotalTasks;
        public Integer CurrentTasks;
        
        #region PUN

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            GameOverCheck();
        }

        //SomeoneDiedEvent triggers this in self asset.
        public void GameOverCheck()
        {
            int totalImpostors = SceneStateManager.Instance.GetImpostorsCount();
            int totalCrewmate = SceneStateManager.Instance.GetCrewmateCount();
            

            if (CurrentTasks.Value == TotalTasks.Value || totalImpostors == 0)
            {
                StartCoroutine(GameScene("Scenes/CrewmateWin"));
            }
            else if(totalImpostors >= totalCrewmate)
            {
                StartCoroutine(GameScene("Scenes/ImpostorWin"));
            }
        }
        
        public IEnumerator GameScene(string scene)
        {
#if UNITY_EDITOR
            yield return new WaitForSeconds(GameConfiguration.ChangeSceneWaitingDebug);
#else
        yield return new WaitForSeconds(GameConfiguration.ChangeSceneWaiting);
#endif
            SceneManager.LoadScene(scene);
        }

        public bool IsImpostor(Player otherPlayer)
        {
            return SceneStateManager.Instance.IsImpostor(otherPlayer);
        }

        #endregion

    }
}
