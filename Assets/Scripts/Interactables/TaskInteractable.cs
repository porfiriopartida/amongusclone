using System;
using DefaultNamespace;
using UnityEngine;

public class TaskInteractable : UseInteractable
{
    public string Task;
    public GameObject TaskPrefab;
    
    public override void Interact()
    {
        throw new InvalidOperationException("Mini games require a source to be interacted with.");
    }
    
    public override void Interact(GameObject source)
    {
        Debug.Log("Interacting as: " + source.name);
        
        GameObject instantiate = Instantiate(TaskPrefab, source.transform, false);
        MiniGameController MiniGameScreen = instantiate.GetComponent<MiniGameController>();
        MiniGameScreen.SetTrigger(this);

        MiniGameScreen.OpenGame();
        
        Debug.Log("Opening Task " + Task);
    }
}
