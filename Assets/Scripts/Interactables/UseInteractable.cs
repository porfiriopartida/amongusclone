using System;
using LopapaGames.Common.Core;
using UnityEngine;

public abstract class UseInteractable : MonoBehaviour, IInteractable
{
    public bool CrewmateExclusive;

    public GameEvent UseInteractEvent;
    public GameEvent NoUseInteractEvent;
    
    void Start()
    {
        if (CrewmateExclusive && SceneStateManager.Instance.IsImpostor())
        {
            this.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
    }
    public abstract void Interact();
    public bool CanInteract()
    {
        return this.gameObject && this.gameObject.activeSelf;
    }

    public void Interact(object param)
    {
        if (CanInteract())
        {
            this.Interact((GameObject) param);
        }
    }

    public virtual void Interact(GameObject source)
    {
        if (CanInteract())
        {
            Interact();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        MomongoController momongoController = other.gameObject.GetComponent<MomongoController>();
        if (momongoController != null && momongoController.photonView.IsMine)
        {
            UseInteractEvent.Raise();
            other.gameObject.GetComponent<MomongoController>().SetUseInteractable(this);
        }
    }

    private void OnDestroy()
    {
        NoUseInteractEvent.Raise();
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