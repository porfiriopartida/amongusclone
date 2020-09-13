using System;
using LopapaGames.Common.Core;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

//If you ever need to have more killables this must be an abstract class instead.
public class Killable : MonoBehaviour, IInteractable
{
    public MomongoController MomongoController;
    
    public GameEvent KillableEvent;
    public GameEvent NotKillable;

    private void OnTriggerStay2D(Collider2D other)
    {
        MomongoController otherMomongoController = other.gameObject.GetComponent<MomongoController>();
        if (otherMomongoController != null && otherMomongoController.photonView.IsMine && SceneStateManager.Instance.IsImpostor(otherMomongoController.photonView.Owner))
        {
            KillableEvent.Raise();
            otherMomongoController.SetKillable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        MomongoController otherMomongoController = other.gameObject.GetComponent<MomongoController>();
        if (otherMomongoController != null && otherMomongoController.photonView.IsMine && SceneStateManager.Instance.IsImpostor(otherMomongoController.photonView.Owner))
        {
            NotKillable.Raise();
            otherMomongoController.SetKillable(null);
        }
    }

    public void Interact()
    {
        MomongoController.GetKilled();
    }

    public void Interact(object param)
    {
        // this.Interact((Player) param);
        Interact();
    }

    // public void Interact(Player player)
    // {
    //     // MomongoController.Die(player);
    //     MomongoController.Die();
    // }
}
