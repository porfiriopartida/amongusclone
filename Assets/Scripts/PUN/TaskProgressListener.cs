using DefaultNamespace;
using ExitGames.Client.Photon;
using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;

namespace PUN
{
    public class TaskProgressListener : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public Float Progress;
        public Integer TotalTasks;
        public Integer CurrentAmount;

        public GameEvent TaskProgressUpdated;
        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == EventsConstants.TASK_COMPLETE)
            {
                CurrentAmount.Value++;
                Progress.Value = ((float)CurrentAmount.Value / (float)(TotalTasks.Value));
                TaskProgressUpdated.Raise();
            }
        }
    }
}