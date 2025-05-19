using UnityEngine;

public class PawMovement : MonoBehaviour
{
    public float moveTime;

    private Transform _player;
    private Vector2 _startPos;
    private bool _moveTowards;
    private bool _moveAway;
    private float _movementTimer;
    private bool _hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _startPos = transform.localPosition;
        _moveTowards = false;
        _moveAway = false;
        _movementTimer = 0f;
        _hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveTowards)
        {
            _movementTimer += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.localPosition, _player.localPosition, _movementTimer / moveTime);
            if (_movementTimer >= moveTime) _hit = true;
        }
        else if (_moveAway)
        {
            _movementTimer += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.localPosition, _startPos, _movementTimer / moveTime);
        }
    }

    public bool Hit()
    {
        return _hit;
    }

    public void MoveTowards()
    {
        if (!_moveTowards) _movementTimer = 0f;
        _moveTowards = true;
        _moveAway = false;
    }

    public void MoveAway()
    {
        if (!_moveAway) _movementTimer = 0f;
        _moveAway = true;
        _moveTowards = false;
    }

    public void Restart()
    {
        _moveTowards = false;
        _moveAway = false;
        _movementTimer = 0f;
        _hit = false;
        transform.position = _startPos;
    }
}
