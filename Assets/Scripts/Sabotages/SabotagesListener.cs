using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class SabotagesListener : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventsConstants.SABOTAGE_LIGHT) {
            SabotagesManager.Instance.TurnOffLights();
        } else if (photonEvent.Code == EventsConstants.FIX_LIGHT) {
            SabotagesManager.Instance.TurnOnLights();
        }
    }
}
