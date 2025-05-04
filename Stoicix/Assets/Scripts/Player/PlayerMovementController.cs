using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Vector2 _moveInput;
    private bool _takeInput;

    private Rigidbody2D _rigidbody;
    private PlayerInputHandler _inputHandler;
    private PlayerAnimationController _animationController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _animationController = GetComponent<PlayerAnimationController>();
        _takeInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        _moveInput = _takeInput ? _inputHandler.MoveInput.normalized : Vector2.zero;
        _rigidbody.linearVelocity = new Vector2(_moveInput.x, _moveInput.y) * _speed;

        _animationController.AnimateMovement(_moveInput);
        TurnCheck();
    }

    public void Freeze()
    {
        _takeInput = false;
    }

    public void Unfreeze()
    {
        _takeInput = true;
    }

    private void TurnCheck()
    {
        switch (_moveInput.x)
        {
            case > 0:
                Turn(true);
                break;
            case < 0:
                Turn(false);
                break;
        }
    }

    private void Turn(bool side) // true = right, false = left
    {
        transform.localScale = side ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
