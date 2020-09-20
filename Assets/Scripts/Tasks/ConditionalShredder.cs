using LopapaGames.Common.Core;
using UnityEngine;

public class ConditionalShredder : MonoBehaviour
{
    public GameEvent MiniGameTrigger;
    public string[] tagNames;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tagName in tagNames)
        {
            if (collision.tag == tagName)
            {
                Destroy(collision.gameObject);
                if (MiniGameTrigger != null)
                {
                    MiniGameTrigger.Raise();
                }
            }
        }
    }
}