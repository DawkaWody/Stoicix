using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour, IMinigame
{
    [SerializeField] private Paws _paws;
    [SerializeField] private Transform _endPos;
    [SerializeField] private float _endDelay = 0.3f;

    private Vector2 _startPos;
    private bool _gameEnding;
    private IInteractable.EmptyCallback _onGameEnd;

    private PlayerMovementController _movementController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _movementController = GetComponentInParent<PlayerMovementController>();
        _movementController.disableHorizontal = true;
        _gameEnding = false;
        _startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition(_movementController.GetInput().y);
        if (_paws.IsLost() && !_gameEnding) StartCoroutine(GameEndWithDelay(false));
        if (transform.position.y <= _endPos.position.y && !_gameEnding) StartCoroutine(GameEndWithDelay(true));
    }

    public void UpdatePosition(float yInput)
    {
        transform.localPosition = yInput switch
        {
            > 0 => new Vector3(transform.localPosition.x, 0.26f, transform.localPosition.z),
            < 0 => new Vector3(transform.localPosition.x, -0.49f, transform.localPosition.z),
            _ => transform.localPosition
        };
    }

    public void StartGame(IInteractable.EmptyCallback onGameEnd)
    {
        _onGameEnd = onGameEnd;
    }

    private IEnumerator GameEndWithDelay(bool gameWon)
    {
        _gameEnding = true;
        UiManager.Instance.ShowWinScreen();
        yield return new WaitForSeconds(_endDelay);
        _onGameEnd?.Invoke();
        if (!gameWon)
        {
            transform.position = _startPos;
            _movementController.transform.position = _startPos;
            _paws.Restart();
        }
        else
        {
            _movementController.disableHorizontal = false;
            GameManager.Instance.CompleteQuest();
        }
        _gameEnding = false;
    }
}
