using LopapaGames.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class CooldownRenderer : MonoBehaviour
{
    public CooldownManager cooldownManager;
    public string Cooldown;
    public Text text;
    
    void Update()
    {
        int timer = Mathf.RoundToInt(cooldownManager.GetTimer(Cooldown));
        if (timer <= 0)
        {
            text.text = "";
        }
        else
        {
            text.text = timer.ToString();
        }
    }
}
