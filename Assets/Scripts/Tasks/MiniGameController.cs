using LopapaGames.ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class MiniGameController : MonoBehaviour
    {
        public CooldownManager CooldownManager;
        private TaskInteractable taskInteractable;
        public abstract void StartGame();

        private void Awake()
        {
            SceneStateManager.Instance.EnteringMiniGame();
        }

        public void OpenGame()
        {
            SceneStateManager.Instance.EnteringMiniGame();
            
            gameObject.SetActive(true);
            StartGame();
        }

        public void CloseGame()
        {
            Destroy(this.gameObject);
        }


        public virtual void AwardProgress()
        {
            Debug.Log("Awarding progress for mini game " + gameObject.name);
            Destroy(taskInteractable.gameObject, .5f);
            SceneStateManager.Instance.AwardProgress();
        }
        
        #region Events
        private void OnEnable()
        {
            SceneStateManager.Instance.EnteringMiniGame();
        }

        private void OnDisable()
        {
            SceneStateManager.Instance.LeavingMiniGame();
        }

        private void OnDestroy()
        {
            SceneStateManager.Instance.LeavingMiniGame();
        }
        

        #endregion

        public void SetTrigger(TaskInteractable taskInteractable)
        {
            this.taskInteractable = taskInteractable;
        }
    }
}