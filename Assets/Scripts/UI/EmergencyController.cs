using UnityEngine;
using UnityEngine.UI;

public class EmergencyController : MonoBehaviour
{
    public Image Reporter;
    public void SetFounder(PlayerWrapper reporter)
    {
        if (reporter != null)
        {
            Reporter.color = reporter.Color;
        }
    }
}
