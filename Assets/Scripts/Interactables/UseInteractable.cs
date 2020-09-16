using LopapaGames.Common.Core;
using UnityEngine;

public abstract class UseInteractable : MonoBehaviour, IInteractable
{
    public bool CrewmateExclusive;
    
    void Start()
    {
        if (CrewmateExclusive && SceneStateManager.Instance.IsImpostor())
        {
            this.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
    public abstract void Interact();
    public void Interact(object param)
    {
        this.Interact((GameObject) param);
    }

    public virtual void Interact(GameObject source)
    {
        Interact();
    }

    public GameEvent UseInteractEvent;
    public GameEvent NoUseInteractEvent;
    private void OnTriggerStay2D(Collider2D other)
    {
        MomongoController momongoController = other.gameObject.GetComponent<MomongoController>();
        if (momongoController != null && momongoController.photonView.IsMine)
        {
            UseInteractEvent.Raise();
            other.gameObject.GetComponent<MomongoController>().SetUseInteractable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MomongoController momongoController = other.gameObject.GetComponent<MomongoController>();
        if (momongoController != null && momongoController.photonView.IsMine)
        {
            NoUseInteractEvent.Raise();
            other.gameObject.GetComponent<MomongoController>().SetUseInteractable(null);
        }
    }
}