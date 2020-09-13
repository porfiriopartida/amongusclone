using UnityEngine;

public class LightOnOffInteractable : UseInteractable
{
    public bool isOn;
    private SpriteRenderer _spriteRenderer;
    
    public Color OnColor = Color.yellow;
    public Color OffColor = Color.black;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        RefreshColor();
    }

    void RefreshColor()
    {
        _spriteRenderer.color = isOn ? OnColor : OffColor;
    }

    public override void Interact()
    {
        isOn = !isOn;
        RefreshColor();
    }
}
