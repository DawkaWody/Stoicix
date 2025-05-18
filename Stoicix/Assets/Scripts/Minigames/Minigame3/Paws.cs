using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Paws : MonoBehaviour
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    private float _timer;
    private float _animationTimer;
    private bool _resetTimer;
    private bool _attacking;
    private bool _gameLost;
    
    private Animator _animator;
    private PlayerMovementController _playerMovementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovementController = GetComponentInParent<PlayerMovementController>();
        _timer = 0f;
        _resetTimer = true;
        _attacking = false;
        _gameLost = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = _playerMovementController.GetInput();

        if (_resetTimer)
        {
            _timer = Random.Range(_minWaitTime, _maxWaitTime);
            _resetTimer = false;
        }

        if (movement.sqrMagnitude > 0) _timer -= Time.deltaTime;
        Debug.Log(_timer);

        if (!(_timer <= 0f)) return;
        _animationTimer += Time.deltaTime;
        if (_animationTimer > 1f && movement.sqrMagnitude == 0)
        {
            Debug.Log("reset");
            _animator.SetTrigger("reset");
            _animator.ResetTrigger("start");
            _resetTimer = true;
            _animationTimer = 0f;
        }
        else _animator.SetTrigger("start");
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            _gameLost = true;
        }
    }

    public void Restart()
    {
        _timer = 0f;
        _resetTimer = true;
        _attacking = false;
        _gameLost = false;
        _animator.ResetTrigger("start");
        _animator.ResetTrigger("reset");
        _animator.Play("None");
    }

    public bool IsLost()
    {
        return _gameLost;
    }
}
