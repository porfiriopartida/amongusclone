using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnMaskOn : MonoBehaviour
{
    void Start()
    {
        GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
}
