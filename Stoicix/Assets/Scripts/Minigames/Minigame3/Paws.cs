using UnityEngine;

public class Paws : MonoBehaviour
{
    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;
    [SerializeField] private PawMovement[] _paws;
    [SerializeField] private float _movementTime;

    private float _timer;
    private float _movingTimer;
    private bool _resetTimer;
    private bool _gameLost;
    
    private PlayerMovementController _playerMovementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovementController = GetComponentInParent<PlayerMovementController>();
        _timer = 0f;
        _resetTimer = true;
        _gameLost = false;
        foreach (PawMovement pawMovement in _paws) pawMovement.moveTime = _movementTime;
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

        if (!(_timer <= 0f)) return;
        _movingTimer += Time.deltaTime;
        Debug.Log("Moving (paws.cs): " + _movingTimer);
        if (_movingTimer > 1f && movement.sqrMagnitude == 0)
        {
            Debug.Log("reset");
            foreach (PawMovement pawMovement in _paws) pawMovement.MoveAway();
            _resetTimer = true;
            _movingTimer = 0f;
        }
        else
        {
            foreach (PawMovement pawMovement in _paws)
            {
                pawMovement.MoveTowards();
                _gameLost = _gameLost || pawMovement.Hit();
            }
        }
    }

    public void Restart()
    {
        _timer = 0f;
        _resetTimer = true;
        _gameLost = false;
        foreach (PawMovement pawMovement in _paws) pawMovement.Restart();
    }

    public bool IsLost()
    {
        return _gameLost;
    }
}
