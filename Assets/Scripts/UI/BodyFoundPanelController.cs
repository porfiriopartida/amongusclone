using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BodyFoundPanelController : MonoBehaviour
    {
        public Image Reporter;
        public Image Body;
        public void SetFounder(Player reporter)
        {
            if (reporter != null)
            {
                Reporter.color = SceneStateManager.Instance.GetColor(reporter);
            }
        }

        public void SetBody(Player body)
        {
            if (body != null)
            {
                Body.color = SceneStateManager.Instance.GetColor(body);
            }
        }
    }
}