using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BarLift : MonoBehaviour, IMinigame
{
    [SerializeField] private float _targetCps;
    [SerializeField] private float _duraton;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    [SerializeField] private float _forceMultiplierBecauseChatgptSucks = 1f;
    [SerializeField] private float _endDelay = 0.3f;

    [Header("Border")]
    [SerializeField] private GameObject BorderObject;
    [SerializeField] private float fadeDuration = 0.3f;

    private SpriteRenderer _spriteRenderer;
    private bool _fadeInStarted;

    private float _startY;
    private float _clickForce;
    private float _gameTimer;
    private bool _gameActive;
    private bool _gameStarted;
    private bool _skipNextGravityCheck;
    private bool _gameWon;
    private int _clickCountThisSecond;
    private float _cpsTimer;
    private IInteractable.EmptyCallback _onGameEnd;

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
        _gameActive = false;
        _gameStarted = false;
        _skipNextGravityCheck = false;
        _gameWon = false;

        if (BorderObject != null)
        {
            BorderObject.SetActive(true);
            _spriteRenderer = BorderObject.GetComponent<SpriteRenderer>();

            if (_spriteRenderer != null)
            {
                Color c = _spriteRenderer.color;
                c.a = 0f;
                _spriteRenderer.color = c;
            }

            BorderObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameActive) StartCoroutine(GravityResetCheck());

        if (transform.position.y > _maxY)
        {
            transform.position = new Vector3(transform.position.x, _maxY, transform.position.z);
            _rigidbody.linearVelocity = Vector2.zero;
        }

        if (!_gameActive) return;

        //cps for fun
        _cpsTimer += Time.deltaTime;

        if (_playerInputHandler.MouseWasPressed)
        {
            _clickCountThisSecond++;

            _rigidbody.gravityScale = 1f;
            _skipNextGravityCheck = true; // skip the next gravity check
            _rigidbody.AddForce(Vector2.up * _clickForce, ForceMode2D.Impulse);
        }

        // Log CPS every second
        if (_cpsTimer >= 1f)
        {
            Debug.Log($"CPS: {_clickCountThisSecond}");
            _clickCountThisSecond = 0;
            _cpsTimer = 0f;
        }

        StartCoroutine(GravityResetCheck());

        if (!_gameStarted)
        {
            if (transform.position.y > _minY)
            {
                _gameStarted = true;

                if (!_fadeInStarted && BorderObject != null)
                {
                    BorderObject.SetActive(true);
                    StartCoroutine(FadeSpriteAlpha(0f, 1f, fadeDuration));
                    _fadeInStarted = true;
                }
            }
            else return;
        }

        if (transform.position.y < _minY)
        {
            _gameActive = false;
            _gameWon = false;
            Debug.Log("Loser hahahhahahhahahahahahahahahahhahahahahahhahahahhahahahah");
        }
        else
        {
            _gameTimer -= Time.deltaTime;
            if (!(_gameTimer <= 0)) return;
            _gameActive = false;
            _gameWon = true;
            Debug.Log("Winner yayayayayayayayayayayayayayyayyyayayay");
        }

        StartCoroutine(GameEndWithDelay(_gameWon));
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
        transform.position = new Vector3(transform.position.x, _startY, transform.position.z);
        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.gravityScale = 0f;
    }

    private IEnumerator GameEndWithDelay(bool gameWon)
    {
        if (_spriteRenderer != null) yield return StartCoroutine(FadeSpriteAlpha(1f, 0f, fadeDuration));

        if (BorderObject != null) BorderObject.SetActive(false);

        yield return new WaitForSeconds(_endDelay);
        _onGameEnd?.Invoke();

        if (!gameWon)
        {
            _gameTimer = _duraton;
            _rigidbody.gravityScale = 0f;
            _gameActive = false;
            _gameStarted = false;
            _skipNextGravityCheck = false;
            _gameWon = false;
            _fadeInStarted = false;
        }
        else
        {
            GameManager.Instance.MarkQuestCompletedInScene();
        }
    }

    public void StartGame(IInteractable.EmptyCallback onGameEnd)
    {
        _gameActive = true;
        _onGameEnd = onGameEnd;

        //border
        if (_spriteRenderer != null)
        {
            Color c = _spriteRenderer.color;
            c.a = 0f;
            _spriteRenderer.color = c;
        }

        if (BorderObject != null) BorderObject.SetActive(true);
        _fadeInStarted = false;
    }

    private IEnumerator FadeSpriteAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color color = _spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        _spriteRenderer.color = new Color(color.r, color.g, color.b, to);
    }
}
