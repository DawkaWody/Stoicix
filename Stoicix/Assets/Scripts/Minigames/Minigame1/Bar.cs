using System;
using UnityEngine;

[RequireComponent(typeof(BarLift))]
public class Bar : MonoBehaviour, IInteractable
{
    public int InteractPriority { get; set; }

    private bool _interactable;

    private BarLift _barLift;

    void Start()
    {
        _barLift = GetComponent<BarLift>();
        _interactable = false;
    }

    public void Interact(IInteractable.EmptyCallback onInteractionEnd = null)
    {
        if (_interactable) _barLift.StartGame(onInteractionEnd);
    }

    public void SetInteractable()
    {
        _interactable = true;
    }
}
