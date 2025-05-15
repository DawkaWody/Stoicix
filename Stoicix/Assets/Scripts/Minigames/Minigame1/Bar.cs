using System;
using UnityEngine;

[RequireComponent(typeof(BarLift))]
public class Bar : MonoBehaviour, IInteractable
{
    public int InteractPriority { get; set; }

    private BarLift _barLift;

    void Start()
    {
        _barLift = GetComponent<BarLift>();
    }

    public void Interact(IInteractable.EmptyCallback onInteractionEnd = null)
    {
        Debug.Log("skibidey game start");
        _barLift.StartGame(onInteractionEnd);
    }
}
