using UnityEngine;

public class OnTriggerGravityUpdate : MonoBehaviour
{
    public string TargetTag;
    public float InGravity = 0;
    public float OutGravity = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(TargetTag))
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = InGravity;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(TargetTag))
        {
            other.gameObject.GetComponent<Rigidbody2D>().gravityScale = OutGravity;
        }
    }
}
