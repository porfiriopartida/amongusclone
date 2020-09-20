using DefaultNamespace;
using UnityEngine;

namespace Tasks.ScoopingDogeFish
{
    public class ScoopingDogfishTask : MiniGameController
    {
        public int DogfishCount = 3;
        public override void StartGame()
        {
            //Spawn Dogfish here?
        }
        public void DogfishConsumed()
        {
            if (--DogfishCount == 0)
            {
                AwardProgress();
                CloseGame();
            }
        }
    }
}