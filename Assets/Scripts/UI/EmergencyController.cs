using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class EmergencyController : MonoBehaviour
{
    public Image Reporter;
    public void SetFounder(Player reporter)
    {
        if (reporter != null)
        {
            Reporter.color = SceneStateManager.Instance.GetColor(reporter);
        }
    }
}
