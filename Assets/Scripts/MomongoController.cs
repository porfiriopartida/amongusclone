using LopapaGames.Common.Core;
using LopapaGames.ScriptableObjects;
using Photon.Pun;
using Photon.Realtime;
using PUN;
using UnityEngine;

// [RequireComponent(typeof(CharacterController))]
public class MomongoController : MonoBehaviour
{
    //   private Rigidbody2D _rigidbody;
    public Float MovSpeed;
    public Float VisionRange;
    private float RealSpeed;

    public GameObject VisionMask;
    //private CharacterController _characterController;
    
    private Vector2 _cacheDirection;
    
    private Rigidbody2D _rigidbody2D;

    private IInteractable _useInteractable;
    private IInteractable _reportableInteractable;
    private IInteractable _killable;

    public PhotonView photonView;

    public GameObject DeadBodyPrefab;

    private Animator _animator;

    public GameEvent SomeoneDiedEvent;

    public CooldownManager CooldownManager;
    public GameObject AdminSprite;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // photonView = PhotonView.Get(this);
        _animator = GetComponent<Animator>();
        
        LoadCustomizations();
    }

    private float _killCooldown;
    private void LoadCustomizations()
    {
        float movSpeed = (float) PhotonNetwork.CurrentRoom.CustomProperties["MovSpeed"];
        RealSpeed = MovSpeed.Value * movSpeed;
        
        float visionRange = 1;
        
        if (SceneStateManager.Instance.IsImpostor())
        {
            visionRange = (float) PhotonNetwork.CurrentRoom.CustomProperties["ImpostorVisionRange"];
            _killCooldown = (float) PhotonNetwork.CurrentRoom.CustomProperties["KillCooldown"];
        }
        else
        {
            visionRange = (float) PhotonNetwork.CurrentRoom.CustomProperties["VisionRange"];
        }

        AdjustVision(visionRange);
        ResetCooldowns();
    }

    public void ShowMask()
    {
        VisionMask.SetActive(true);
    }
    public void AdjustVision(float range)
    {
        float newVal = VisionRange.Value * range;
        VisionMask.transform.localScale = new Vector3(newVal, newVal, 0f);
    }

    public void Stop()
    {
        Move(Vector2.zero);
    }

    #region Interactables
    public void Interact()
    {
        if (_useInteractable != null)
        {
            _useInteractable.Interact(gameObject);
        }
    }
    
    public void Report()
    {
        if (_reportableInteractable != null)
        {
            _reportableInteractable.Interact(PhotonNetwork.LocalPlayer);
        }
    }
    
    public void Sabotage()
    {
        if (!IsImpostor())
        {
            return;
        }
        // Debug.Log("Not implemented, Sabotage!");
        // if (CooldownManager.GetTimer("SabotageCooldown") > 0)
        // {
        //     return;
        // }
        // CooldownManager.AddTimer("SabotageCooldown", _sabotageCooldown);
        // _killable.Interact(photonView.Owner);
    }
    
    public void Kill()
    {
        if (!IsImpostor())
        {
            return;
        }
        
        if (CooldownManager.GetTimer("KillCooldown") > 0)
        {
            return;
        }
        CooldownManager.AddTimer("KillCooldown", _killCooldown);
        _killable.Interact(photonView.Owner);
    }

    private bool IsImpostor()
    {
        return SceneStateManager.Instance.IsImpostor(photonView.Owner);
    }

    public void ResetCooldowns()
    {
        CooldownManager.AddTimer("KillCooldown", _killCooldown);
    }

    public void SetKillable(IInteractable killable)
    {
        _killable = killable;
    }

    public void SetUseInteractable(IInteractable interactable)
    {
        _useInteractable = interactable;
    }

    public void SetReportable(IInteractable interactable)
    {
        _reportableInteractable = interactable;
    }

    #endregion
    #region PUNRPC
    public void Move(Vector2 direction)
    {
        if (_cacheDirection == direction)
        {
            return;
        }
        
        _cacheDirection = direction;

        // PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_Move", RpcTarget.AllViaServer, direction);
    }
    
    public void GetKilled()
    {
        photonView.RPC("RPC_Die", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer, photonView.Owner);
    }
    
    [PunRPC]
    public void RPC_Die(Player killer, Player deadPlayer)
    {
        if (deadPlayer.IsLocal)
        {
            //TODO: Show big dying animation.
        }

        GameObject go = Instantiate(DeadBodyPrefab, transform.position, Quaternion.identity);
        go.GetComponent<DeadBodyInteractable>().SetPlayer(deadPlayer);
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        go.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
        spriteRenderer.enabled = false;

        Die();
    }

    public void Die()
    {
        //TODO: Make sure this doesn't call the event for already dead players.
        GhostMe();

        if (PhotonNetwork.IsMasterClient)
        {
            SomeoneDiedEvent.Raise();
        }
    }

    public void GhostMe()
    {
        PlayerSetup playerSetup = GetComponent<PlayerSetup>();
        playerSetup.SetPlayerUI(); //This is needed so the killer doesn't kill the ghost again.
        playerSetup.KillRangeCollider.SetActive(false);
        _animator.SetBool("IsDead", true);
        
        SceneStateManager.Instance.SetIsAlive(photonView.Owner, false);
        SceneStateManager.Instance.SyncGhosts();
    }

    [PunRPC]
    public void RPC_Move(Vector2 direction)
    {
        if (_rigidbody2D)
        {
            _rigidbody2D.velocity = direction * RealSpeed;
        }
    }
    #endregion
}
