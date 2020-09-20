using LopapaGames.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DogefishController : MonoBehaviour
{
    public float Speed;
    private CooldownManager _cooldownManager;
    private Rigidbody2D _myRigidbody;
    private Vector2 LastSpeed;
    private bool IsPaused = false;
    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _cooldownManager = gameObject.AddComponent<CooldownManager>();
        ResetSpeed();
    }

    private void Update()
    {
        if (_cooldownManager.GetTimer("MoveChange") <= 0)
        {
            ResetSpeed();
            _cooldownManager.AddTimer("MoveChange", 3f);
        }
    }

    private void ResetSpeed()
    {
        if (IsPaused)
        {
            return;
        }

        int rndX = Random.Range(0, 2) == 0 ? 1:-1;
        transform.localScale = new Vector3(-rndX, 1, 0);
        _myRigidbody.velocity = new Vector2(rndX * Speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Dogfish Collision with " + other.gameObject.name);
        if (other.gameObject.CompareTag("minigame_player"))
        {
            LastSpeed = _myRigidbody.velocity;
            _myRigidbody.velocity = Vector2.zero;
            IsPaused = true;
        }
        else
        {
            ResetSpeed();
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("minigame_player"))
        {
            _myRigidbody.velocity = LastSpeed;
            IsPaused = false;
        }
    }
}
