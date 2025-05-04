using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovementController))]
public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private Transform _interactPoint;
    [SerializeField] private float _interactRadius = 2f;
    [SerializeField] private LayerMask _interactableLayers;

    private PlayerInputHandler _inputHandler;
    private PlayerMovementController _movementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _movementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_inputHandler.InteractWasPressed) return;
        List<IInteractable> interactables = Physics2D.OverlapCircleAll(_interactPoint.position, _interactRadius, _interactableLayers)
            .Select(collider => collider.GetComponent<IInteractable>())
            .Where(interactable => interactable != null)
            .ToList();
        IInteractable interactable = interactables.FirstOrDefault(i =>
            i.InteractPriority == interactables.Max(interactable => interactable.InteractPriority));
        _movementController.Freeze();
        interactable?.Interact(_movementController.Unfreeze);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_interactPoint.position, _interactRadius);
    }
}
