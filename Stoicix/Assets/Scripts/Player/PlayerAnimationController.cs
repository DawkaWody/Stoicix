using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimateMovement(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= 0)
        {
            _animator.SetBool("isMoving", false);
            return;
        }
        _animator.SetBool("isMoving", true);
        _animator.SetFloat("directionX", moveDirection.x);
        _animator.SetFloat("directionY", moveDirection.y);
    }
}
