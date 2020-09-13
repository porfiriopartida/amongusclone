using LopapaGames.Common.Core;
using Photon.Realtime;
using UnityEngine;

public abstract class ReportInteractable : MonoBehaviour, IInteractable
{
    public abstract void Interact();
    public void Interact(object param)
    {
        this.Interact((Player) param);
    }

    public abstract void Interact(Player player);

    public GameEvent ReportInteractEvent;
    public GameEvent NoReportInteractEvent;
    private void OnTriggerStay2D(Collider2D other)
    {
        MomongoController momongoController = other.gameObject.GetComponent<MomongoController>();
        if (momongoController != null && momongoController.photonView.IsMine)
        {
            ReportInteractEvent.Raise();
            momongoController.SetReportable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MomongoController momongoController = other.gameObject.GetComponent<MomongoController>();
        if (momongoController != null && momongoController.photonView.IsMine)
        {
            NoReportInteractEvent.Raise();
            other.gameObject.GetComponent<MomongoController>().SetReportable(null);
        }
    }
}