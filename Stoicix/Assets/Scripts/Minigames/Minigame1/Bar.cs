using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bar : MonoBehaviour
{
    [SerializeField] private float _targetCps;
    [SerializeField] private float _duraton;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    [SerializeField] private float _forceMultiplierBecauseChatgptSucks = 1f;

    private float _startY;
    private float _clickForce;
    private float _gameTimer;
    private bool _gameActive;
    private bool _gameStarted;
    private bool _skipNextGravityCheck;

    private Rigidbody2D _rigidbody;
    private PlayerInputHandler _playerInputHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInputHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputHandler>();

        _startY = transform.position.y;
        _clickForce = _rigidbody.mass * (_maxY - _minY) / _targetCps;
        _clickForce *= _forceMultiplierBecauseChatgptSucks; // force to apply to the object when clicked
        _gameTimer = _duraton;
        _rigidbody.gravityScale = 0f;
        _gameActive = true;
        _gameStarted = false;
        _skipNextGravityCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > _maxY)
        {
            Debug.Log("Clamping Y");
            transform.position = new Vector3(transform.position.x, _maxY, transform.position.z);
            _rigidbody.linearVelocity = Vector2.zero;
        }

        if (!_gameActive) return;

        if (_playerInputHandler.MouseWasPressed)
        {
            _rigidbody.gravityScale = 1f;
            _skipNextGravityCheck = true; // skip the next gravity check
            Debug.Log(_rigidbody.gravityScale);
            _rigidbody.AddForce(Vector2.up * _clickForce, ForceMode2D.Impulse);
        }

        StartCoroutine(GravityResetCheck());

        if (!_gameStarted)
        {
            if (transform.position.y > _minY)
            {
                _gameStarted = true;
            }
            else return;
        }

        if (transform.position.y < _minY)
        {
            _gameActive = false;
            Debug.Log("Loser hahahhahahhahahahahahahahahahhahahahahahhahahahhahahahah");
        }
        else
        {
            _gameTimer -= Time.deltaTime;
            if (!(_gameTimer <= 0)) return;
            _gameActive = false;
            Debug.Log("Winner yayayayayayayayayayayayayayyayyyayayay");
        }

        // Disable gravity after the game ends
        _rigidbody.gravityScale = 0f;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private IEnumerator GravityResetCheck()
    {
        if (_skipNextGravityCheck)
        {
            yield return new WaitForEndOfFrame();
            _skipNextGravityCheck = false;
        }
        yield return new WaitForEndOfFrame();
        if (!(transform.position.y <= _startY)) yield break;
        Debug.Log("Gravity reset because _startY reached");
        transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.gravityScale = 0f;
    }
}
