using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private string _moveActionName = "Move";
    [SerializeField] private string _interactActionName = "Interact";
    [SerializeField] private string _mousePressActionName = "MousePress";

    [HideInInspector] public Vector2 MoveInput { get; private set; }
    [HideInInspector] public bool InteractWasPressed { get; private set; }
    [HideInInspector] public bool MouseWasPressed { get; private set; }

    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _mousePressAction;

    private PlayerInput _playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions[_moveActionName];
        _interactAction = _playerInput.actions[_interactActionName];
        _mousePressAction = _playerInput.actions[_mousePressActionName];
    }

    // Update is called once per frame
    void Update()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();

        InteractWasPressed = _interactAction.WasPressedThisFrame();
        MouseWasPressed = _mousePressAction.WasPressedThisFrame();
    }
}