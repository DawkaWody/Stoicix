using UnityEngine;

public class Philosopher : MonoBehaviour, IInteractable
{
    [SerializeField] private Monolog _monolog;
    [SerializeField] private int _minigameScene;

    public int InteractPriority { get; set; } = 3;

    public void Interact(IInteractable.EmptyCallback OnInteractionEnd)
    {
        if (!_monolog.Next()) OnInteractionEnd?.Invoke();
    }
}
