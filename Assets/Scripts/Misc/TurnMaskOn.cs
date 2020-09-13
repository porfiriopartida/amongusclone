using UnityEngine;

public class TurnMaskOn : MonoBehaviour
{
    void Start()
    {
        GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}
