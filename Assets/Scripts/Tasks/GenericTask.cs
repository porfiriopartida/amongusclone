using DefaultNamespace;

namespace Tasks
{
    public class GenericTask : MiniGameController
    {
        public bool IsRunning = false;
        private bool _isWin = false;
        public float Length;
        public override void StartGame()
        {
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
        }

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

                    IsRunning = false;
                }
            }
        }
    }
}
