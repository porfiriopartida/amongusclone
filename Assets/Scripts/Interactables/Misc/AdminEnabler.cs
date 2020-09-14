using UnityEngine;

public class AdminEnabler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<MomongoController>().AdminSprite.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<MomongoController>().AdminSprite.SetActive(false);
    }
}
