using UnityEngine;

public interface IInteractable
{
    public int InteractPriority { get; set; }
    void Interact();
}
