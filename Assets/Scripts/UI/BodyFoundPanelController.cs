using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BodyFoundPanelController : MonoBehaviour
    {
        public Image Reporter;
        public Image Body;
        public void SetFounder(PlayerWrapper reporter)
        {
            if (reporter != null)
            {
                Reporter.color = reporter.Color;
            }
        }

        public void SetBody(PlayerWrapper body)
        {
            if (body != null)
            {
                Body.color = body.Color;
            }
        }
    }
}