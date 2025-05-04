using UnityEngine;

public interface IInteractable
{
    public int InteractPriority { get; set; }
    public delegate void EmptyCallback();
    void Interact(EmptyCallback onInteractionEnd=null);
}
