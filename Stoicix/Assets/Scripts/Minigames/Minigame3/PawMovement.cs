using UnityEngine;

public class PawMovement : MonoBehaviour
{
    public float moveTime;

    private Transform _player;
    private Vector2 _startPos;
    private Vector2 _pawOffset;
    private bool _moveTowards;
    private bool _moveAway;
    private float _movementTimer;
    private bool _hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _moveTowards = false;
        _moveAway = false;
        _movementTimer = 0f;
        _hit = false;

        _pawOffset = (Vector2)(transform.position - _player.position);
    }

    // Update is called once per frame
    void Update()
    {
        _startPos = (Vector2)_player.position + _pawOffset;
        if (_moveTowards)
        {
            _movementTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _player.position, _movementTimer / moveTime);
            if (_movementTimer >= moveTime) _hit = true;
        }
        else if (_moveAway)
        {
            _movementTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, _startPos, _movementTimer / moveTime);
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
        _startPos = (Vector2)_player.position + _pawOffset;
        transform.position = _startPos;
    }
}
