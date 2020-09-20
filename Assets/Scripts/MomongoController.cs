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
    public GameEvent HardEvent;
    public Float MovSpeed;
    public Float GhostSpeed;
    public Float VisionRange;
    private float RealSpeed;
    public GameObject MyMapIndicator;

    public GameObject VisionMask;
    //private CharacterController _characterController;
    
    private Vector2 _cacheDirection;
    
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private IInteractable _useInteractable;
    [SerializeField]
    private IInteractable _reportableInteractable;
    [SerializeField]
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
        AdminSprite.SetActive(true);
        
        LoadCustomizations();
    }
    [SerializeField]
    private float _emergencyCooldown;
    [SerializeField]
    private float _killCooldown;
    private float _visionRange;
    private float _blindVisionRange;
    private float _currentVisionRange;
    private void LoadCustomizations()
    {
        float movSpeed = (float) PhotonNetwork.CurrentRoom.CustomProperties["MovSpeed"];
        RealSpeed = MovSpeed.Value * movSpeed;
        
        _visionRange = 1;
        
        if (SceneStateManager.Instance.IsImpostor())
        {
            _visionRange = (float) PhotonNetwork.CurrentRoom.CustomProperties["ImpostorVisionRange"];
            _killCooldown = (float) PhotonNetwork.CurrentRoom.CustomProperties["KillCooldown"];
        }
        else
        {
            _visionRange = (float) PhotonNetwork.CurrentRoom.CustomProperties["VisionRange"];
        }

        _emergencyCooldown = (int) PhotonNetwork.CurrentRoom.CustomProperties["EmergencyCooldown"];
        _blindVisionRange = .20f;
        SceneStateManager.Instance.EmergencyCount = (int) PhotonNetwork.CurrentRoom.CustomProperties["EmergencyCount"]; 
        AdjustVision(_visionRange);
        ResetCooldowns();
    }
    
    public void HideMask()
    {
        VisionMask.SetActive(false);
    }
    public void ShowMask()
    {
        VisionMask.SetActive(true);
    }
    public void AdjustVision(float range)
    {
        if (range >= _blindVisionRange)
        {
            _currentVisionRange = range;
            float newVal = VisionRange.Value * range;
            VisionMask.transform.localScale = new Vector3(newVal, newVal, 0f);
        }
    }

    public void Stop()
    {
        Move(Vector2.zero);
    }

    #region Interactables
    public void Interact()
    {
        if (_useInteractable == null || _useInteractable.CanInteract())
        {
        }

        _useInteractable?.Interact(gameObject);
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
        
        UIMapManager.Instance.ToggleSabotageMap();
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
        SceneStateManager.Instance.CooldownManager.AddTimer("EmergencyCooldown", _emergencyCooldown);
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
            Debug.Log(deadPlayer.NickName + ", you just got killed by " + killer.NickName);
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
        GhostMe();

        if (PhotonNetwork.IsMasterClient)
        {
            SomeoneDiedEvent.Raise();
        }

        if (photonView.IsMine)
        {
            HardEvent.Raise();
        }
    }

    public void GhostMe()
    {
        PlayerSetup playerSetup = GetComponent<PlayerSetup>();
        playerSetup.SetPlayerUI(); //This is needed so the killer doesn't kill the ghost again.
        playerSetup.KillRangeCollider.SetActive(false);
        _animator.SetBool("IsDead", true);
        AdminSprite.SetActive(false);
        
        float movSpeed = (float) PhotonNetwork.CurrentRoom.CustomProperties["MovSpeed"];
        RealSpeed = GhostSpeed.Value * movSpeed;
        
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

    private bool _blinding = false;
    private float _nextBlind = .20f;
    private void FixedUpdate()
    {
        if (_blinding)
        {
            
            if (_nextBlind <= 0)
            {
                if (_currentVisionRange > _blindVisionRange)
                {
                    float step = _blindVisionRange/5;
                    AdjustVision(_currentVisionRange - step);
                }
                else
                {
                    _blinding = false;
                }
                _nextBlind = .20f;
            }
            else
            {
                _nextBlind -= Time.deltaTime;
            }

        }
    }

    public void TurnOffLights()
    {
        if (IsImpostor())
        {
            return;
        }

        _blinding = true;
    }
    public void TurnOnLights()
    {
        _blinding = false;
        AdjustVision(_visionRange);
    }
}
