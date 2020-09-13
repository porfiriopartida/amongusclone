using System.Collections;
using DefaultNamespace;
using LopapaGames.ScriptableObjects;

namespace Tasks
{
    public class GenericTask : MiniGameController
    {
        public bool IsRunning = false;
        private bool _isWin = false;
        public float Length;
        public CooldownManager CooldownManager;
        public override void StartGame()
        {
            // OpenGame();
            ResetState();
        }

        private void ResetState()
        {
            IsRunning = true;
            _isWin = false;
        }

        public void SetWin()
        {
            _isWin = true;
            CooldownManager.AddTimer("WaitTime", Length);
            // StartCoroutine(DelayedWin());
        }

        // private IEnumerator DelayedWin()
        // {
        //     
        // }

        private void Update()
        {
            if (IsRunning)
            {
                if (_isWin)
                {
                    if (CooldownManager.GetTimer("WaitTime") > 0)
                    {
                        //Wait for the delay.
                        return;
                    }

                    AwardProgress();
                    CloseGame();
                    //TODO: Network - Notify Task completed
                
                    //Task completed awards points, returns Input Controller and checks win condition.

                    IsRunning = false;
                }
            }
        }
    }
}
